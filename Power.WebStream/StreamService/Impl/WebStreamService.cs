using PowerStream.Core;
using System.Net;

namespace Power.WebStream
{
    [AliasName("Web")]
    public class WebStreamService : IProccessService
    {
        public void Send(ProccessInfo info)
        {
            WebClient webClient = new WebClient();
            info.SourceLink = webClient.DownloadString("http://183.131.138.61:9080/video?type=Rtmp&sourceid=0000000000200000000000001244435:0000000000140000000000001231833:192.168.10.148:331100");
        }
    }
}