using Harmonic.Controllers;
using Harmonic.Service;
using PowerStream.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStream.Server
{
    [NeverRegister]
    internal class PowerSmartController : WebSocketPlayController
    {
        private IStreamService StreamService { get; set; }
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
            var ffmpegProcess = PowerManager.FFmpegProcessList.FirstOrDefault(s => s.StreamName == this.StreamName);
            if (ffmpegProcess != null)
            {
                result = !ffmpegProcess.PID.HasValue;
                var wsConn = ffmpegProcess.WsConnection?.FirstOrDefault(c => c.WebSocketConnection.ConnectionInfo.ClientIpAddress == this.Session.WebSocketConnection.ConnectionInfo.ClientIpAddress);
                if (wsConn == null)
                {
                    ffmpegProcess.WsConnection.Add(this.Session);
                }
            }

            if (ffmpegProcess == null || result)
            {
                StreamService.Send(this.StreamName);

                ffmpegProcess = PowerManager.FFmpegProcessList.FirstOrDefault(s => s.StreamName == this.StreamName);
                while (!ffmpegProcess.PushStreamTime.HasValue)
                {
                    Thread.Sleep(PowerOption.WaitTime * 1000);
                }
            }

            return base.OnConnect();
        }
    }
}