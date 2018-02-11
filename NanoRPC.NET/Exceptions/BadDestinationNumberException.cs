using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadDestinationNumberException : Exception
    {
        public string Destination { get; private set; }

        public BadDestinationNumberException() : base("Bad destination number!")
        {
            Destination = "";
        }

        public BadDestinationNumberException(string destination) : base("Bad destination number '" + destination + "'!")
        {
            Destination = destination;
        }

        public BadDestinationNumberException(string destination, Exception inner) : base("Bad destination number '" + destination + "'!", inner)
        {
            Destination = destination;
        }

        protected BadDestinationNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Destination = "";
        }
    }
}
