using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BlockUnreceivableException : Exception
    {
        public BlockUnreceivableException() { }
        public BlockUnreceivableException(string message) : base(message) { }
        public BlockUnreceivableException(string message, Exception inner) : base(message, inner) { }
        protected BlockUnreceivableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
