using Harmonic.Controllers;
using Harmonic.Networking.WebSocket;
using Harmonic.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStreamServer
{
    [NeverRegister]
    internal class PowerSmartController : WebSocketPlayController
    {
        private IStreamService StreamService;
        private PowerOptions PowerOption { get; set; }

        public PowerSmartController(PublisherSessionService publisherSessionService, RecordService recordService, IStreamService streamService, PowerOptions option)
            : base(publisherSessionService, recordService)
        {
            this.StreamService = streamService;
            this.PowerOption = option;
        }

        public override Task OnConnect()
        {
            var result = false;
            var ffmpegProcess = Power.FFmpegProcessList.FirstOrDefault(s => s.StreamName == this.StreamName);
            if (ffmpegProcess != null)
            {
                result = !ffmpegProcess.PID.HasValue;
                if (ffmpegProcess.PID.HasValue)
                {
                    var hasProcess = Process.GetProcessesByName("ffmpeg")?.FirstOrDefault(t => t.Id == ffmpegProcess.PID.Value);
                    if (hasProcess == null)
                    {
                        ffmpegProcess.PID = null;
                        result = true;
                    }
                    else
                    {
                        ffmpegProcess.LastActiveTime = DateTime.Now;
                    }
                }
                var wsConn = ffmpegProcess.WsConnection?
                    .FirstOrDefault(c => c.WebSocketConnection.ConnectionInfo.ClientIpAddress == this.Session.WebSocketConnection.ConnectionInfo.ClientIpAddress);
                if (wsConn == null)
                {
                    ffmpegProcess.WsConnection.Add(this.Session);
                }
            }

            if (ffmpegProcess == null || result)
            {
                StreamService.Send(this.StreamName);
                Thread.Sleep(PowerOption.WaitTime * 1000);
            }

            return base.OnConnect();
        }
    }
}