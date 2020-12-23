using Harmonic.Networking.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tianyi;
using Xabe.FFmpeg;

namespace PowerStreamServer
{
    public class TykdStreamService : IStreamService
    {
        private static object lockObj = new object();
        private PowerOptions PowerOption { get; set; }
        private TianyiService TyService { get; set; }

        public TykdStreamService(PowerOptions option, TianyiService service)
        {
            PowerOption = option;
            TyService = service;
        }

        public void Send(string streamName)
        {
            var data = Power.FFmpegProcessList.FirstOrDefault(t => t.StreamName == streamName);
            if (!(data == null || data.CreateTime == DateTime.MinValue))
            {
                if (data.CreateTime.AddSeconds(PowerOption.WaitTime) > DateTime.Now)
                {
                    return;
                }
            }
            Monitor.Enter(lockObj);
            data = Power.FFmpegProcessList.FirstOrDefault(t => t.StreamName == streamName);
            if (!(data == null || !data.PID.HasValue))
            {
                return;
            }
            string globalParam = PowerOption.Sources.GlobalParam;
            string inputParam = PowerOption.Sources.InputParam;
            string outputParam = PowerOption.Sources.OutputParam;
            string outputLink = PowerOption.Sources.ForwardLink;
            string soruceLink = TyService.GetExtGetPlayUrlHX(streamName, 0);

            IConversion iConversion = FFmpeg.Conversions.New();
            iConversion.OnDataReceived += IConversion_OnDataReceived;
            Console.WriteLine($"ffmpeg {globalParam} {inputParam} -i \"{soruceLink}\" {outputParam} \"{outputLink}{streamName}\"");
            data = new StreamConnection()
            {
                LastActiveTime = DateTime.Now,
                StreamName = streamName,
                WsConnection = new List<WebSocketSession>()
            };
            Power.FFmpegProcessList.Add(data);
            iConversion.Start($"{globalParam} {inputParam} -i \"{soruceLink}\" {outputParam} \"{outputLink}{streamName}\"", t =>
            {
                data.PID = t;
                data.LastActiveTime = DateTime.Now;
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]FFmpeg流推送到：\n{outputLink}{streamName}\n进程ID为：{ data.PID }\n-----------------");
            });

            // 检测是否开启FFmpeg进程
            while (!data.PID.HasValue)
            {
                Thread.Sleep(3000);
            }
            data.CreateTime = DateTime.Now;
            Monitor.Exit(lockObj);
        }

        private void IConversion_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (PowerOption.FFmpegLog)
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}