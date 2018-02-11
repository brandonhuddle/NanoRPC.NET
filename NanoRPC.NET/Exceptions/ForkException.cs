using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class ForkException : Exception
    {
        public ForkException() { }
        public ForkException(string message) : base(message) { }
        public ForkException(string message, Exception inner) : base(message, inner) { }
        protected ForkException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
