using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadAccountNumberException : Exception
    {
        public string AccountNumber { get; private set; }

        public BadAccountNumberException() : base("Bad account number!")
        {
            AccountNumber = "";
        }

        public BadAccountNumberException(string account) : base("Bad account number '" + account + "'!")
        {
            AccountNumber = account;
        }

        public BadAccountNumberException(string account, Exception inner) : base("Bad account number '" + account + "'!", inner)
        {
            AccountNumber = account;
        }

        protected BadAccountNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AccountNumber = "";
        }
    }
}
