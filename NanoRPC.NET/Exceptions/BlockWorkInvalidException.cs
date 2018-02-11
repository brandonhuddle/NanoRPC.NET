using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BlockWorkInvalidException : Exception
    {
        public string Work { get; private set; }

        public BlockWorkInvalidException() : base("Block work is invalid!")
        {
            Work = "";
        }

        public BlockWorkInvalidException(string work) : base("Block work '" + work + " is invalid!")
        {
            Work = work;
        }

        public BlockWorkInvalidException(string work, Exception inner) : base("Block work '" + work + " is invalid!", inner)
        {
            Work = work;
        }

        protected BlockWorkInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Work = "";
        }
    }
}
