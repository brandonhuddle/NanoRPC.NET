using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BlockNotFoundException : Exception
    {
        public string BlockHash { get; private set; }

        public BlockNotFoundException() : base("Block not found!")
        {
            BlockHash = "";
        }

        public BlockNotFoundException(string blockHash) : base("Block '" + blockHash + "' not found!")
        {
            BlockHash = blockHash;
        }

        public BlockNotFoundException(string blockHash, Exception inner) : base("Block '" + blockHash + "' not found!", inner)
        {
            BlockHash = blockHash;
        }

        protected BlockNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BlockHash = "";
        }
    }
}
