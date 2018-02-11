using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadPublicKeyException : Exception
    {
        public string PublicKey { get; private set; }

        public BadPublicKeyException() : base("Bad public key!")
        {
            PublicKey = "";
        }

        public BadPublicKeyException(string publicKey) : base("Bad public key '" + publicKey + "!")
        {
            PublicKey = publicKey;
        }

        public BadPublicKeyException(string publicKey, Exception inner) : base("Bad public key '" + publicKey + "!", inner)
        {
            PublicKey = publicKey;
        }

        protected BadPublicKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PublicKey = "";
        }
    }
}
