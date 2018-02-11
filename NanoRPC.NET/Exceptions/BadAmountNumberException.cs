using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadAmountNumberException : Exception
    {
        public BigInteger Amount { get; private set; }

        public BadAmountNumberException() : base("Bad amount number!")
        {
            Amount = 0;
        }

        public BadAmountNumberException(BigInteger amount) : base("Bad amount number '" + amount + "'!")
        {
            Amount = amount;
        }

        public BadAmountNumberException(BigInteger amount, Exception inner) : base("Bad amount number '" + amount + "'!", inner)
        {
            Amount = amount;
        }

        protected BadAmountNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Amount = 0;
        }
    }
}
