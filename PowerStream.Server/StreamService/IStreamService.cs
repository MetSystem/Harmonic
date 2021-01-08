using PowerStream.Core;

namespace PowerStream.Server
{
    public interface IStreamService
    {
        void Send(string streamName);
    }

  
}