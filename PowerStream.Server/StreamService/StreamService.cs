using Harmonic.Networking.WebSocket;
using PowerStream.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xabe.FFmpeg;

namespace PowerStream.Server
{
    public class StreamService : IStreamService
    {
        private static object lockObj = new object();

        private PowerOptions PowerOption { get; set; }

        public StreamService(PowerOptions option)
        {
            PowerOption = option;
        }

        public void Send(string streamName)
        {
            Monitor.Enter(lockObj);
            try
            {
                var streamInfo = PowerOption.Sources.Data.FirstOrDefault(t => t.Name == streamName);
                var info = new StreamInfo()
                {
                    StreamName = streamName,
                    GlobalParam = string.IsNullOrEmpty(streamInfo.GlobalParam) ? PowerOption.Sources.GlobalParam : streamInfo.GlobalParam,
                    InputParam = string.IsNullOrEmpty(streamInfo.InputParam) ? PowerOption.Sources.InputParam : streamInfo.InputParam,
                    SourceLink = streamInfo.SourceLink,
                    OutputParam = string.IsNullOrEmpty(streamInfo.OutputParam) ? PowerOption.Sources.OutputParam : streamInfo.OutputParam,
                    OutputLink = string.IsNullOrEmpty(streamInfo.ForwardLink) ? PowerOption.Sources.ForwardLink : streamInfo.ForwardLink
                };

                var serivce = IoCHelper.ResolveNamed<IProccessService>(PowerOption.StreamType);
                serivce.Send(info);
                var data = PowerManager.FFmpegProcessList.FirstOrDefault(t => t.StreamName == streamName);
                if (!(data == null || data.PID == null || data.LastActiveTime == DateTime.MinValue))
                {
                    data.LastActiveTime = DateTime.Now;
                    return;
                }

                data = new StreamConnection()
                {
                    LastActiveTime = DateTime.Now,
                    StreamName = streamName,
                    WsConnection = new List<WebSocketSession>()
                };

                PowerManager.FFmpegProcessList.Add(data);
                IConversion iConversion = FFmpeg.Conversions.New();
                iConversion.OnDataReceived += data.IConversion_OnDataReceived;
                iConversion.OnProgress += data.IConversion_OnProgress;
                data.StreamInfo = info;
                Console.WriteLine($"ffmpeg {data.Command}");
                iConversion.Start(data.Command, data.StartAction);

                data.DataReceivedEvent = (e, args) =>
                {
                    if (PowerOption.FFmpegLog)
                    {
                        Console.WriteLine(args.Data);
                    }
                };
                data.InitEvent = (t) =>
                {
                    IoCHelper.ResolveNamed<IProccessService>(PowerOption.StreamType)?.Send(t);
                };

                //iConversion.Start(data.Command, t =>
                //{
                //    data.PID = t;
                //    data.LastActiveTime = DateTime.Now;
                //    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]FFmpeg进程ID为：{ data.PID }-----------------");
                //});

                while (!data.PID.HasValue)
                {
                    Thread.Sleep(500);
                }
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        //private void IConversion_OnProgress(object sender, Xabe.FFmpeg.Events.ConversionProgressEventArgs args)
        //{
        //    var data = PowerManager.FFmpegProcessList.FirstOrDefault(t => t.PID == args.ProcessId);
        //    if (data.LastActiveTime.AddSeconds(5) < DateTime.Now)
        //    {
        //        data.LastActiveTime = DateTime.Now;
        //    }

        //    if (!data.PushStreamTime.HasValue)
        //    {
        //        data.PushStreamTime = DateTime.Now;
        //    }
        //}

        //private void IConversion_OnDataReceived(object sender, DataReceivedEventArgs e)
        //{


        //    if (PowerOption.FFmpegLog)
        //    {
        //        Console.WriteLine(e.Data);
        //    }
        //}
    }
}