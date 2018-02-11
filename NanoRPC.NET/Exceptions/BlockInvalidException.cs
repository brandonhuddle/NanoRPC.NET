using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BlockInvalidException : Exception
    {
        public string Block { get; private set; }

        public BlockInvalidException() : base("Block is invalid!")
        {
            Block = "";
        }

        public BlockInvalidException(string block) : base("Block '" + block + "' is invalid!")
        {
            Block = block;
        }

        public BlockInvalidException(string block, Exception inner) : base("Block '" + block + "' is invalid!", inner)
        {
            Block = block;
        }

        protected BlockInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Block = "";
        }
    }
}
