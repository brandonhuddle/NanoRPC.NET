using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class BadWalletNumberException : Exception
    {
        public string WalletNumber { get; private set; }

        public BadWalletNumberException() : base("Bad wallet number!")
        {
            WalletNumber = "";
        }

        public BadWalletNumberException(string wallet) : base("Bad wallet number '" + wallet + "'!")
        {
            WalletNumber = wallet;
        }

        public BadWalletNumberException(string wallet, Exception inner) : base("Bad wallet number '" + wallet + "'!", inner)
        {
            WalletNumber = wallet;
        }

        protected BadWalletNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            WalletNumber = "";
        }
    }
}
