using Autofac;
using Harmonic.Hosting;
using Microsoft.Extensions.Configuration;
using PowerStream.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace PowerStream.Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = IoCHelper.GetAssignableFrom<IModuleService>();
            services?.ForEach(t => t.Init());

            var powerOptions = PowerManager.Configuration.GetSection("Server").Get<PowerOptions>();
            var executablesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FFmpeg");
            if (!string.IsNullOrEmpty(powerOptions.FFmpegPath))
            {
                executablesPath = powerOptions.FFmpegPath;
            }

            FFmpeg.SetExecutablesPath(executablesPath);
            RtmpServer server = new RtmpServerBuilder()
                .UseStartup<Startup>()
                .UseWebSocket(c =>
                {
                    c.Register<PowerSmartController>();
                    c.BindEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), powerOptions.Port);
                })
                .Build();

            var thread = new Thread((d) =>
            {
                while (true)
                {
                    Parallel.ForEach(PowerManager.FFmpegProcessList, (item) =>
                    {
                        if (item.PushStreamTime.HasValue && item.LastActiveTime.AddSeconds(5) <= DateTime.Now)
                        {
                            item.LastActiveTime = DateTime.Now;
                            item.PushStreamTime = null;
                            item.InitEvent(item.StreamInfo);
                            Process.GetProcessesByName("ffmpeg")?.FirstOrDefault(p => p.Id == item.PID.Value)?.Kill();
                            var iConversion = FFmpeg.Conversions.New();
                            iConversion.OnDataReceived += item.IConversion_OnDataReceived;
                            iConversion.OnProgress += item.IConversion_OnProgress;
                            iConversion.Start(item.Command, item.StartAction);
                        }

                        if (!item.PushStreamTime.HasValue && item.LastActiveTime.AddSeconds(10) <= DateTime.Now)
                        {
                            Process.GetProcessesByName("ffmpeg")?.FirstOrDefault(p => p.Id == item.PID.Value)?.Kill();
                            item.PID = null;
                            item.PushStreamTime = null;
                            item.LastActiveTime = DateTime.Now;
                        }
                    });

                    Thread.Sleep(5 * 1000);
                }
            });

            thread.IsBackground = true;
            thread.Start();
            var tsk = server.StartAsync();
            tsk.Wait();
        }
    }
}