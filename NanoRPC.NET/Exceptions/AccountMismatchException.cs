using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class AccountMismatchException : Exception
    {
        public AccountMismatchException() { }
        public AccountMismatchException(string message) : base(message) { }
        public AccountMismatchException(string message, Exception inner) : base(message, inner) { }
        protected AccountMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
