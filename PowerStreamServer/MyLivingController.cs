using Harmonic.Controllers;
using Harmonic.Controllers.Living;
using Harmonic.Rpc;

namespace PowerStreamServer
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