using Harmonic.Networking.WebSocket;
using PowerStream.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xabe.FFmpeg.Events;

namespace PowerStream.Server
{
    public class StreamConnection
    {
        /// <summary>
        /// FFmpeg进程ID
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 流名称
        /// </summary>
        public string StreamName { get; set; }

        /// <summary>
        /// WS连接
        /// </summary>
        public List<WebSocketSession> WsConnection { get; set; }

        public StreamInfo StreamInfo { get; set; }

        public DateTime LastActiveTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? PushStreamTime { get; set; }

        public Action<StreamInfo> InitEvent;

        public Action<object, DataReceivedEventArgs> DataReceivedEvent;

        public Action<int> StartAction => t =>
        {
            this.PID = t;
            this.LastActiveTime = DateTime.Now;
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] FFmpeg进程{ this.PID }=> {this.Command}");
        };

        public string Command
        {
            get
            {
                return $"{StreamInfo.GlobalParam} {StreamInfo.InputParam} -i \"{StreamInfo.SourceLink}\" {StreamInfo.OutputParam} \"{StreamInfo.OutputLink}{StreamInfo.StreamName}\"";
            }
        }

        public void IConversion_OnProgress(object sender, ConversionProgressEventArgs args)
        {
            if (this.LastActiveTime.AddSeconds(3) < DateTime.Now)
            {
                this.LastActiveTime = DateTime.Now;
            }

            if (!this.PushStreamTime.HasValue)
            {
                this.PushStreamTime = DateTime.Now;
            }
        }


        public void IConversion_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceivedEvent?.Invoke(sender, e);
        }
    }
}