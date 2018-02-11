using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidStartingAccountException : Exception
    {
        public string Account { get; private set; }

        public InvalidStartingAccountException() : base("Invalid starting account!")
        {
            Account = "";
        }

        public InvalidStartingAccountException(string account) : base("Invalid starting account '" + account + "'!")
        {
            Account = account;
        }

        public InvalidStartingAccountException(string account, Exception inner) : base("Invalid starting account '" + account + "'!", inner)
        {
            Account = account;
        }

        protected InvalidStartingAccountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Account = "";
        }
    }
}
