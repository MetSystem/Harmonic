using PowerStream.Core;
using Tianyi;

namespace PowerStreamServer
{
    [AliasName("Tykd")]
    public class TykdStreamService : IProccessService
    {
        private static object lockObj = new object();
        private TianyiService TyService { get; set; }

        public TykdStreamService(TianyiService service)
        {
            TyService = service;
        }

        public void Send(StreamInfo info, PowerOptions powerOptions)
        {
            info.GlobalParam = powerOptions.Sources.GlobalParam;
            info.InputParam = powerOptions.Sources.InputParam;
            info.OutputParam = powerOptions.Sources.OutputParam;
            info.OutputLink = powerOptions.Sources.ForwardLink;
            info.SourceLink = TyService.GetExtGetPlayUrlHX(info.StreamName, 0);
        }
    }
}