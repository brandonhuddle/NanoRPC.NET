using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidIPAddressException : Exception
    {
        public string IPAddress { get; private set; }

        public InvalidIPAddressException() : base("Invalid IP address!")
        {
            IPAddress = "";
        }

        public InvalidIPAddressException(string ipAddress) : base("Invalid IP address '" + ipAddress + "'!")
        {
            IPAddress = ipAddress;
        }

        public InvalidIPAddressException(string ipAddress, Exception inner) : base("Invalid IP address '" + ipAddress + "'!", inner)
        {
            IPAddress = ipAddress;
        }

        protected InvalidIPAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            IPAddress = "";
        }
    }
}
