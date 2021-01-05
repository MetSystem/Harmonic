namespace PowerStream.Core
{
    public class PowerOptions
    {
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// FFmpegPath
        /// </summary>
        public string? FFmpegPath { get; set; }
        /// <summary>
        /// 视频源
        /// </summary>
        public SourceOption Sources { get; set; }
        public bool FFmpegLog { get; set; }
        public string StreamType { get; set; }
        public string? FFmpegConfig { get; set; }
        public bool FFmpegDisplay { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        public int WaitTime { get; set; }
    }

    public class SourceOption
    {
        public Streams[] Data { get; set; }
        public string? ForwardLink { get; set; }
        public string? GlobalParam { get; set; }
        public string? InputParam { get; set; }
        public string? OutputParam { get; set; }
    }

    public class Streams
    {
        public string? Name { get; set; }
        public string? SourceLink { get; set; }
        public string? ForwardLink { get; set; }
        public string? GlobalParam { get; set; }
        public string? InputParam { get; set; }
        public string? OutputParam { get; set; }
    }
}
