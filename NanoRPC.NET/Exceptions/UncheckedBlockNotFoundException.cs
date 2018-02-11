using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class UncheckedBlockNotFoundException : Exception
    {
        public string BlockHash { get; private set; }

        public UncheckedBlockNotFoundException() : base("Unchecked block not found!")
        {
            BlockHash = "";
        }

        public UncheckedBlockNotFoundException(string blockHash) : base("Unchecked block '" + blockHash + "' not found!")
        {
            BlockHash = blockHash;
        }

        public UncheckedBlockNotFoundException(string blockHash, Exception inner) : base("Unchecked block '" + blockHash + "' not found!", inner)
        {
            BlockHash = blockHash;
        }

        protected UncheckedBlockNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BlockHash = "";
        }
    }
}
