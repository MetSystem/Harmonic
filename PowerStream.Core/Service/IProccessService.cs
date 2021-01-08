namespace PowerStream.Core
{
    public class StreamInfo
    {
        public string StreamName { get; set; }
        public string GlobalParam { get; set; }
        public string InputParam { get; set; }
        public string SourceLink { get; set; }
        public string OutputParam { get; set; }
        public string OutputLink { get; set; }
    }

    public interface IProccessService : ISingletonDependency
    {
        void Send(StreamInfo info);
    }
}
