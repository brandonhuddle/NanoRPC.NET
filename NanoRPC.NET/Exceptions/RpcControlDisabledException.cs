using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class RpcControlDisabledException : Exception
    {
        public RpcControlDisabledException() : base("RPC control is disabled") { }
        public RpcControlDisabledException(string message) : base(message) { }
        public RpcControlDisabledException(string message, Exception inner) : base(message, inner) { }
        protected RpcControlDisabledException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
