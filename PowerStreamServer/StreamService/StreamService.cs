using System;
using System.Linq;
using Xabe.FFmpeg;

namespace PowerStreamServer
{
    public class StreamService : IStreamService
    {
        private PowerOptions PowerOption;

        public StreamService(PowerOptions option)
        {
            PowerOption = option;
        }

        public void Send(string streamName)
        {
            var streamInfo = PowerOption.Sources.Data.FirstOrDefault(t => t.Name == streamName);

            var globalParam = string.IsNullOrEmpty(streamInfo.GlobalParam) ? PowerOption.Sources.GlobalParam : streamInfo.GlobalParam;
            var inputParam = string.IsNullOrEmpty(streamInfo.InputParam) ? PowerOption.Sources.InputParam : streamInfo.InputParam;
            var soruceLink = streamInfo.SourceLink;
            var outputParam = string.IsNullOrEmpty(streamInfo.OutputParam) ? PowerOption.Sources.OutputParam : streamInfo.OutputParam;
            var outputLink = string.IsNullOrEmpty(streamInfo.ForwardLink) ? PowerOption.Sources.ForwardLink : streamInfo.ForwardLink;

            var data = Power.FFmpegProcessList.FirstOrDefault(t => t.StreamName == streamName);
            if (!(data == null || data.LastActiveTime == DateTime.MinValue))
            {
                data.LastActiveTime = DateTime.Now;
                return;
            }

            data = new StreamConnection()
            {
                LastActiveTime = DateTime.Now,
                StreamName = streamName,
                WsConnection = new System.Collections.Generic.List<Harmonic.Networking.WebSocket.WebSocketSession>()
            };
            IConversion iConversion = FFmpeg.Conversions.New();
            iConversion.Start($"{globalParam} {inputParam} -i {soruceLink} {outputParam} {outputLink}{streamName}",t=> {
                data.PID = t;
                data.LastActiveTime = DateTime.Now;
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]FFmpeg流推送到：\n{outputLink}{streamName}\n进程ID为：{ data.PID }\n-----------------");
            });
        }
    }
}