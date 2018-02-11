using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class GapPreviousBlockException : Exception
    {
        public GapPreviousBlockException() { }
        public GapPreviousBlockException(string message) : base(message) { }
        public GapPreviousBlockException(string message, Exception inner) : base(message, inner) { }
        protected GapPreviousBlockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
