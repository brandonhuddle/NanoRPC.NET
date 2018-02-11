using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class AccountNotFoundInWalletException : Exception
    {
        public string AccountNumber { get; private set; }
        public string WalletNumber { get; private set; }

        public AccountNotFoundInWalletException() : base("Account number not found in wallet!")
        {
            AccountNumber = "";
            WalletNumber = "";
        }

        public AccountNotFoundInWalletException(string account, string wallet) : base("Account '" + account + "' not found in wallet '" + wallet + "'!")
        {
            AccountNumber = account;
            WalletNumber = wallet;
        }

        public AccountNotFoundInWalletException(string account, string wallet, Exception inner) : base("Account '" + account + "' not found in wallet '" + wallet + "'!", inner)
        {
            AccountNumber = account;
            WalletNumber = wallet;
        }

        protected AccountNotFoundInWalletException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AccountNumber = "";
            WalletNumber = "";
        }
    }
}
