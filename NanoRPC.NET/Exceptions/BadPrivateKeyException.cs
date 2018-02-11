using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadPrivateKeyException : Exception
    {
        public string PrivateKey { get; private set; }

        public BadPrivateKeyException() : base("Bad private key!")
        {
            PrivateKey = "";
        }

        public BadPrivateKeyException(string privateKey) : base("Bad private key '" + privateKey + "'!")
        {
            PrivateKey = privateKey;
        }

        public BadPrivateKeyException(string privateKey, Exception inner) : base("Bad private key '" + privateKey + "'!", inner)
        {
            PrivateKey = privateKey;
        }

        protected BadPrivateKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PrivateKey = "";
        }
    }
}
