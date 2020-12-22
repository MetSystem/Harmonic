namespace PowerStreamServer
{
    public interface IStreamService
    {
        void Send(string streamName);
    }
}