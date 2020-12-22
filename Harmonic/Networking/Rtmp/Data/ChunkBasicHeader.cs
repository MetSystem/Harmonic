namespace Harmonic.Networking.Rtmp.Data
{
    internal class ChunkBasicHeader
    {
        public ChunkHeaderType RtmpChunkHeaderType { get; set; }
        public uint ChunkStreamId { get; set; }
    }
}