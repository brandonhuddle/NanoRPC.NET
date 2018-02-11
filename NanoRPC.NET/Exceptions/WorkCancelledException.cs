using System;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class WorkCancelledException : Exception
    {
        public WorkCancelledException() : base("Work generation cancelled") { }
        public WorkCancelledException(string message) : base(message) { }
        public WorkCancelledException(string message, Exception inner) : base(message, inner) { }
        protected WorkCancelledException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
