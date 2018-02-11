using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadWorkException : Exception
    {
        public string Work { get; private set; }

        public BadWorkException() : base("Bad work!")
        {
            Work = "";
        }

        public BadWorkException(string work) : base("Bad work '" + work + "!")
        {
            Work = work;
        }

        public BadWorkException(string work, Exception inner) : base("Bad work '" + work + "!", inner)
        {
            Work = work;
        }

        protected BadWorkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Work = "";
        }
    }
}
