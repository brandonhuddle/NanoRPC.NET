using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class InvalidAccountNumberException : Exception
    {
        public string Account { get; private set; }

        public InvalidAccountNumberException() : base("Invalid account number!")
        {
            Account = "";
        }

        public InvalidAccountNumberException(string account) : base("Invalid account number '" + account + "'!")
        {
            Account = account;
        }

        public InvalidAccountNumberException(string account, Exception inner) : base("Invalid account number '" + account + "'!", inner)
        {
            Account = account;
        }

        protected InvalidAccountNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Account = "";
        }
    }
}
