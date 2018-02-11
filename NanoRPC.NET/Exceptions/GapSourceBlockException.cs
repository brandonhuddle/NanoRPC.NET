using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class GapSourceBlockException : Exception
    {
        public GapSourceBlockException() { }
        public GapSourceBlockException(string message) : base(message) { }
        public GapSourceBlockException(string message, Exception inner) : base(message, inner) { }
        protected GapSourceBlockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
