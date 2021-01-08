using PowerStream.Core;
using System;

namespace PowerStream.Server.Impl
{
    [AliasName("Default")]
    public class DefaultStreamService : IProccessService
    {
        private PowerOptions PowerOption { get; set; }

        public DefaultStreamService(PowerOptions option)
        {
            PowerOption = option;
        }

        public void Send(StreamInfo info)
        {
            throw new NotImplementedException();
        }
    }
}