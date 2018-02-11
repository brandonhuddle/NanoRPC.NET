using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class SourceNotFoundException : Exception
    {
        public string SourceNumber { get; private set; }

        public SourceNotFoundException() : base("Source number not found!")
        {
            SourceNumber = "";
        }

        public SourceNotFoundException(string sourceNumber) : base("Source number '" + sourceNumber + "' not found!")
        {
            SourceNumber = sourceNumber;
        }

        public SourceNotFoundException(string sourceNumber, Exception inner) : base("Source number '" + sourceNumber + "' not found!", inner)
        {
            SourceNumber = sourceNumber;
        }

        protected SourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SourceNumber = "";
        }
    }
}
