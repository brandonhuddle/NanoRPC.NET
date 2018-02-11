using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public string AccountNumber { get; private set; }

        public AccountNotFoundException() : base("Account number not found!")
        {
            AccountNumber = "";
        }

        public AccountNotFoundException(string account) : base("Account '" + account + "' not found!")
        {
            AccountNumber = account;
        }

        public AccountNotFoundException(string account, Exception inner) : base("Account '" + account + "' not found!", inner)
        {
            AccountNumber = account;
        }

        protected AccountNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AccountNumber = "";
        }
    }
}
