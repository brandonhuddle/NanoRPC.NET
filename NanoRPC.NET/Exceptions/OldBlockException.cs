using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class OldBlockException : Exception
    {
        public OldBlockException() { }
        public OldBlockException(string message) : base(message) { }
        public OldBlockException(string message, Exception inner) : base(message, inner) { }
        protected OldBlockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
