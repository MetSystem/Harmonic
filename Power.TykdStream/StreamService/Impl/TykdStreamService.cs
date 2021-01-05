using PowerStream.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tianyi;

namespace PowerStreamServer
{
    [AliasName("Tykd")]
    public class TykdStreamService : IProccessService
    {
        private static object lockObj = new object();
        private PowerOptions PowerOption { get; set; }
        private TianyiService TyService { get; set; }

        public TykdStreamService(PowerOptions option, TianyiService service)
        {
            PowerOption = option;
            TyService = service;
        }

        public void Send(ProccessInfo info)
        {
            info.GlobalParam = PowerOption.Sources.GlobalParam;
            info.InputParam = PowerOption.Sources.InputParam;
            info.OutputParam = PowerOption.Sources.OutputParam;
            info.OutputLink = PowerOption.Sources.ForwardLink;
            info.SourceLink = TyService.GetExtGetPlayUrlHX(info.StreamName, 0);
        }
    }
}