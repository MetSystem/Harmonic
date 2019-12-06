using Harmonic.Controllers;
using Harmonic.Networking.WebSocket;
using Harmonic.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStreamServer
{
    [NeverRegister]
    internal class PowerSmartController : WebSocketPlayController
    {
        private IStreamService StreamService;

        public PowerSmartController(PublisherSessionService publisherSessionService, RecordService recordService, IStreamService streamService)
            : base(publisherSessionService, recordService)
        {
            this.StreamService = streamService;
        }

        public override Task OnConnect()
        {
            var ffmpegProcess = PowerManager.FFmpegProcessList.Where(s => s.StreamName == this.StreamName).FirstOrDefault();
            if (ffmpegProcess == null)
            {
                PowerManager.FFmpegProcessList.Add(new StreamConnection()
                {
                    StreamName = this.StreamName,
                    WsConnection = new List<WebSocketSession>() { Session }
                });
            }
            else
            {
                var wsConn = ffmpegProcess.WsConnection
                    .Where(c => c.WebSocketConnection.ConnectionInfo.ClientIpAddress == this.Session.WebSocketConnection.ConnectionInfo.ClientIpAddress)
                    .FirstOrDefault();

                if (wsConn == null)
                {
                    ffmpegProcess.WsConnection.Add(this.Session);
                }
            }
            StreamService.Send(this.StreamName);

            return base.OnConnect();
        }
    }
}
