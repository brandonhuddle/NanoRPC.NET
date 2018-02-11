using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadKeyHashException : Exception
    {
        public string KeyHash { get; private set; }

        public BadKeyHashException() : base("Bad key hash number!")
        {
            KeyHash = "";
        }

        public BadKeyHashException(string keyHash) : base("Bad key hash number '" + keyHash + "'!")
        {
            KeyHash = keyHash;
        }

        public BadKeyHashException(string keyHash, Exception inner) : base("Bad key hash number '" + keyHash + "'!", inner)
        {
            KeyHash = keyHash;
        }

        protected BadKeyHashException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            KeyHash = "";
        }
    }
}
