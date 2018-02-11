using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class NotReceiveFromSendException : Exception
    {
        public NotReceiveFromSendException() { }
        public NotReceiveFromSendException(string message) : base(message) { }
        public NotReceiveFromSendException(string message, Exception inner) : base(message, inner) { }
        protected NotReceiveFromSendException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
