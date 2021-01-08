using Harmonic.Controllers;
using Harmonic.Controllers.Living;
using Harmonic.Rpc;

namespace PowerStream.Server
{
    [NeverRegister]
    internal class MyLivingController : LivingController
    {
        [RpcMethod("createStream")]
        public new uint CreateStream()
        {
            var stream = RtmpSession.CreateNetStream<MyLivingStream>();
            return stream.MessageStream.MessageStreamId;
        }
    }
}