using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadSeedException : Exception
    {
        public string Seed { get; private set; }

        public BadSeedException() : base("Bad seed!")
        {
            Seed = "";
        }

        public BadSeedException(string seed) : base("Bad seed '" + seed + "'!")
        {
            Seed = seed;
        }

        public BadSeedException(string seed, Exception inner) : base("Bad seed '" + seed + "'!", inner)
        {
            Seed = seed;
        }

        protected BadSeedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Seed = "";
        }
    }
}
