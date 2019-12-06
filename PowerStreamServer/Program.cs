using Harmonic.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStreamServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RtmpServer server = new RtmpServerBuilder()
                .UseStartup<Startup>()
                .UseWebSocket(c =>
                {
                    c.Register<PowerSmartController>();
                    c.BindEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8189);
                })
                .Build();

            Thread thread = new Thread((d) =>
            {
                while (true)
                {
                    Parallel.ForEach(PowerManager.FFmpegProcessList, (t) =>
                    {
                        var length = t.WsConnection.Where(c => c.WebSocketConnection.IsAvailable).Count();
                        if (length < 0)
                        {
                            var ffmpegProcessList = Process.GetProcessesByName("ffmpeg");
                            var pt = ffmpegProcessList?.Where(c => c.Id == t.PID.Value).FirstOrDefault();
                            if (pt != null)
                            {
                                pt?.Kill();
                            }
                            else
                            {
                                PowerManager.FFmpegProcessList.TryTake(out t);
                                Console.WriteLine($"-----清除进程ID为：{t.PID},视频:{t.StreamName},激活时间：{t.LastActiveTime} 的信息");
                            }
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
