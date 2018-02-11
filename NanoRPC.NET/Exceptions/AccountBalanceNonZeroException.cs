using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class AccountBalanceNonZeroException : Exception
    {
        public string Account { get; private set; }

        public AccountBalanceNonZeroException() : base("Account has non-zero balance!")
        {
            Account = "";
        }

        public AccountBalanceNonZeroException(string account) : base("Account '" + account + "' has non-zero balance!")
        {
            Account = account;
        }

        public AccountBalanceNonZeroException(string account, Exception inner) : base("Account '" + account + "' has non-zero balance!", inner)
        {
            Account = account;
        }

        protected AccountBalanceNonZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Account = "";
        }
    }
}
