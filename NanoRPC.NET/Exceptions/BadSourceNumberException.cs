using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadSourceNumberException : Exception
    {
        public string SourceNumber { get; private set; }

        public BadSourceNumberException() : base("Bad source number!")
        {
            SourceNumber = "";
        }

        public BadSourceNumberException(string sourceNumber) : base("Bad source number '" + sourceNumber + "'!")
        {
            SourceNumber = sourceNumber;
        }

        public BadSourceNumberException(string sourceNumber, Exception inner) : base("Bad source number '" + sourceNumber + "'!", inner)
        {
            SourceNumber = sourceNumber;
        }

        protected BadSourceNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SourceNumber = "";
        }
    }
}
