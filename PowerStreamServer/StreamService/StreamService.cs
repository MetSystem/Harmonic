using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace PowerStreamServer
{
    public class StreamService : IStreamService
    {
        public void Send(string streamName)
        {
            var forwardLink = PowerManager.Configuration.GetSection("ForwardLink").Value;
            var ffmpegPath = PowerManager.Configuration.GetSection("FFmpegPath").Value;
            var FFmpegConfig = PowerManager.Configuration.GetSection("FFmpegConfig").Value;
            var soruceLink = PowerManager.Configuration.GetSection($"RTMP:{streamName}:Source").Value;

            var data = PowerManager.FFmpegProcessList.Where(t => t.StreamName == streamName).FirstOrDefault();
            if (data == null)
            {
                return;
            }
            var isStratProccess = false;
            if (!data.PID.HasValue)
            {
                isStratProccess = true;
            }
            else
            {
                var ffmpegProcessList = Process.GetProcessesByName("ffmpeg");
                var process = ffmpegProcessList?.Where(t => t.Id == data.PID.Value).FirstOrDefault();
                if (process == null)
                {
                    isStratProccess = true;
                }
            }

            if (!isStratProccess)
            {
                return;
            }

            var p = new ProcessStartInfo(fileName: $"{ffmpegPath}/ffmpeg.exe")
            {
                WorkingDirectory = ffmpegPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = $" -i {soruceLink} {FFmpegConfig} \"{forwardLink}{streamName}\""
            };

            var pt = Process.Start(p);
            if (pt == null)
            {
                Console.WriteLine("进程拉起失败！");
                return;
            }
            data.LastActiveTime = DateTime.Now;
            data.PID = pt.Id;
            Console.WriteLine($"开始将流推送到：\n{forwardLink}{streamName}\n进程ID为：{pt.Id}\n-----------------");
            Thread.Sleep(1000);
        }
    }
}
