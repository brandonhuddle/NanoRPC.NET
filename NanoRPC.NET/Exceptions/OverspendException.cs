using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class OverspendException : Exception
    {
        public OverspendException() { }
        public OverspendException(string message) : base(message) { }
        public OverspendException(string message, Exception inner) : base(message, inner) { }
        protected OverspendException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
