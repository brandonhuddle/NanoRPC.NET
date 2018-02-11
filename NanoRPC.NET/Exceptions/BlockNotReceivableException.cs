using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BlockNotReceivableException : Exception
    {
        public string BlockHash { get; set; }

        public BlockNotReceivableException() : base("Block is not receivable!")
        {
            BlockHash = "";
        }

        public BlockNotReceivableException(string block) : base("Block '" + block + "' is not receivable!")
        {
            BlockHash = block;
        }

        public BlockNotReceivableException(string block, Exception inner) : base("Block '" + block + "' is not receivable!", inner)
        {
            BlockHash = block;
        }

        protected BlockNotReceivableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BlockHash = "";
        }
    }
}
