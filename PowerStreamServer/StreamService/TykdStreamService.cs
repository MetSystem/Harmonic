using System;
using System.Diagnostics;
using System.Linq;
using Tianyi;
using Xabe.FFmpeg;

namespace PowerStreamServer
{
    public class TykdStreamService : IStreamService
    {
        private PowerOptions powerOption;
        private TianyiService tyService;

        public TykdStreamService(PowerOptions option, TianyiService service)
        {
            powerOption = option;
            tyService = service;
        }

        public void Send(string streamName)
        {
            var data = Power.FFmpegProcessList.FirstOrDefault(t => t.StreamName == streamName);
            

            string globalParam = powerOption.Sources.GlobalParam;
            string inputParam = powerOption.Sources.InputParam;
            string outputParam = powerOption.Sources.OutputParam;
            string outputLink = powerOption.Sources.ForwardLink;
            data = new StreamConnection()
            {
                LastActiveTime = DateTime.Now,
                StreamName = streamName,
                WsConnection = new System.Collections.Generic.List<Harmonic.Networking.WebSocket.WebSocketSession>()
            };
            string soruceLink = tyService.GetExtGetPlayUrlHX(streamName, 0);

            Power.FFmpegProcessList.Add(data);
            IConversion iConversion = FFmpeg.Conversions.New();
            iConversion.OnDataReceived += IConversion_OnDataReceived;
            Console.WriteLine($"ffmpeg {globalParam} {inputParam} -i {soruceLink} {outputParam} {outputLink}{streamName}");
            iConversion.Start($"{globalParam} {inputParam} -i {soruceLink} {outputParam} {outputLink}{streamName}", t =>
            {
                data.PID = t;
                data.LastActiveTime = DateTime.Now;
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]FFmpeg流推送到：\n{outputLink}{streamName}\n进程ID为：{ data.PID }\n-----------------");
            });
        }

        private void IConversion_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}