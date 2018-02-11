using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class UnableToCreateTransactionAccountException : Exception
    {
        public UnableToCreateTransactionAccountException() { }
        public UnableToCreateTransactionAccountException(string message) : base(message) { }
        public UnableToCreateTransactionAccountException(string message, Exception inner) : base(message, inner) { }
        protected UnableToCreateTransactionAccountException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
