using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidCountLimitException : Exception
    {
        public UInt64 CountLimit { get; private set; }

        public InvalidCountLimitException() : base("Invalid count limit!")
        {
            CountLimit = 0;
        }

        public InvalidCountLimitException(UInt64 countLimit) : base("Invalid count limit '" + countLimit.ToString() + "'!")
        {
            CountLimit = countLimit;
        }

        public InvalidCountLimitException(UInt64 countLimit, Exception inner) : base("Invalid count limit '" + countLimit.ToString() + "'!", inner)
        {
            CountLimit = countLimit;
        }

        protected InvalidCountLimitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CountLimit = 0;
        }
    }
}
