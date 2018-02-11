using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class ErrorProcessingBlockException : Exception
    {
        public string Block { get; private set; }

        public ErrorProcessingBlockException() : base("Error processing block!")
        {
            Block = "";
        }

        public ErrorProcessingBlockException(string block) : base("Error processing block '" + block + "'!")
        {
            Block = block;
        }

        public ErrorProcessingBlockException(string block, Exception inner) : base("Error processing block '" + block + "'!", inner)
        {
            Block = block;
        }

        protected ErrorProcessingBlockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Block = "";
        }
    }
}
