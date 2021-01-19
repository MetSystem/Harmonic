using PowerStream.Core;
using System.Net;

namespace Power.WebStream
{
    [AliasName("Web")]
    public class WebStreamService : IProccessService
    {
        public void Send(StreamInfo info, PowerOptions powerOptions)
        {
            WebClient webClient = new WebClient();
            info.SourceLink = webClient.DownloadString("http://183.131.138.61:9080/video?type=Rtmp&sourceid=" + info.StreamName);
        }
    }
}