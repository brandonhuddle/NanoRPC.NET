using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidBlockHashException : Exception
    {
        public string BlockHash { get; private set; }

        public InvalidBlockHashException() : base("Invalid block hash!")
        {
            BlockHash = "";
        }

        public InvalidBlockHashException(string blockHash) : base("Invalid block hash '" + blockHash + "'!")
        {
            BlockHash = blockHash;
        }

        public InvalidBlockHashException(string blockHash, Exception inner) : base("Invalid block hash '" + blockHash + "'!", inner)
        {
            BlockHash = blockHash;
        }

        protected InvalidBlockHashException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BlockHash = "";
        }
    }
}
