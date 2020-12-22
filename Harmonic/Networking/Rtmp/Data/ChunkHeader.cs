namespace Harmonic.Networking.Rtmp.Data
{
    internal class ChunkHeader
    {
        public ChunkBasicHeader ChunkBasicHeader { get; set; }
        public MessageHeader MessageHeader { get; set; }
        public uint ExtendedTimestamp { get; set; }
    }
}