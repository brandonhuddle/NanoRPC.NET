using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadThresholdNumberException : Exception
    {
        public BigInteger Threshold { get; private set; }

        public BadThresholdNumberException() : base("Bad threshold number!")
        {
            Threshold = 0;
        }

        public BadThresholdNumberException(BigInteger threshold) : base("Bad threshold number '" + threshold + "'!")
        {
            Threshold = threshold;
        }

        public BadThresholdNumberException(BigInteger threshold, Exception inner) : base("Bad threshold number '" + threshold + "'!", inner)
        {
            Threshold = threshold;
        }

        protected BadThresholdNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Threshold = 0;
        }
    }
}
