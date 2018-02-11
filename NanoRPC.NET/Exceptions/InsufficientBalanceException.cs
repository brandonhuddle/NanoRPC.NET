using System;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException() : base("Insufficient balance in source account!") { }
        public InsufficientBalanceException(string message) : base(message) { }
        public InsufficientBalanceException(string message, Exception inner) : base(message, inner) { }
        protected InsufficientBalanceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
