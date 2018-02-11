using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidIndexException : Exception
    {
        public UInt64 Index { get; set; }

        public InvalidIndexException() : base("Invalid index!")
        {
            Index = 0;
        }

        public InvalidIndexException(UInt64 index) : base("Invalid index '" + index + "'!")
        {
            Index = index;
        }

        public InvalidIndexException(UInt64 index, Exception inner) : base("Invalid index '" + index + "'!", inner)
        {
            Index = index;
        }

        protected InvalidIndexException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Index = 0;
        }
    }
}
