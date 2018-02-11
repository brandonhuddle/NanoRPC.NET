using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadSignatureException : Exception
    {
        public BadSignatureException() { }
        public BadSignatureException(string message) : base(message) { }
        public BadSignatureException(string message, Exception inner) : base(message, inner) { }
        protected BadSignatureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
