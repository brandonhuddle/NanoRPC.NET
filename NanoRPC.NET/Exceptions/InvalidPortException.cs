using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidPortException : Exception
    {
        public UInt16 Port { get; private set; }

        public InvalidPortException() : base ("Invalid port!")
        {
            Port = 0;
        }

        public InvalidPortException(UInt16 port) : base("Invalid port '" + port.ToString() + "'!")
        {
            Port = port;
        }

        public InvalidPortException(UInt16 port, Exception inner) : base("Invalid port '" + port.ToString() + "'!", inner)
        {
            Port = port;
        }

        protected InvalidPortException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Port = 0;
        }
    }
}
