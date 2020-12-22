﻿using Harmonic.Networking.Rtmp.Serialization;
using System.Buffers;

namespace Harmonic.Networking.Rtmp.Data
{
    public abstract class Message
    {
        protected ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
        public MessageHeader MessageHeader { get; internal set; } = new MessageHeader();

        internal Message()
        {
        }

        public abstract void Deserialize(SerializationContext context);

        public abstract void Serialize(SerializationContext context);
    }
}