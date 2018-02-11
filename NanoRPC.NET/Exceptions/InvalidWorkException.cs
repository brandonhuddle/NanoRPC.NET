using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidWorkException : Exception
    {
        public string Work { get; private set; }

        public InvalidWorkException() : base("Invalid work!")
        {
            Work = "";
        }

        public InvalidWorkException(string work) : base("Invalid work '" + work + "!")
        {
            Work = work;
        }

        public InvalidWorkException(string work, Exception inner) : base("Invalid work '" + work + "!", inner)
        {
            Work = work;
        }

        protected InvalidWorkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Work = "";
        }
    }
}
