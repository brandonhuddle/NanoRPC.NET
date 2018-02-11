using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadHashException : Exception
    {
        public string Hash { get; private set; }

        public BadHashException() : base("Bad hash number!")
        {
            Hash = "";
        }

        public BadHashException(string hash) : base("Bad hash number '" + hash + "'!")
        {
            Hash = hash;
        }

        public BadHashException(string hash, Exception inner) : base("Bad hash number '" + hash + "'!", inner)
        {
            Hash = hash;
        }

        protected BadHashException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Hash = "";
        }
    }
}
