using Harmonic.Networking.WebSocket;
using System;
using System.Collections.Generic;

namespace PowerStreamServer
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

        public DateTime LastActiveTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}