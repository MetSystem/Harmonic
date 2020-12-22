using Autofac;
using Harmonic.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace PowerStreamServer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var powerOptions = Power.Configuration.GetSection("Server").Get<PowerOptions>();
            FFmpeg.SetExecutablesPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FFmpeg"));
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
                    Parallel.ForEach(Power.FFmpegProcessList, (t) =>
                    {
                        var pt = Process.GetProcessesByName("ffmpeg")?.FirstOrDefault(c => c.Id == t.PID.Value);
                        if (pt == null)
                        { 
                            Power.FFmpegProcessList.TryTake(out t);
                            Console.WriteLine($"-----删除无效缓存:{t.StreamName},激活时间：{t.LastActiveTime} 的信息");
                        }
                    });

                    Thread.Sleep(10 * 1000);
                }
            });

            thread.IsBackground = true;
            //thread.Start();
            var tsk = server.StartAsync();
            tsk.Wait();
        }
    }
}