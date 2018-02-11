using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class WalletNotFoundException : Exception
    {
        public string WalletNumber { get; private set; }

        public WalletNotFoundException() : base("Wallet not found!")
        {
            WalletNumber = "";
        }

        public WalletNotFoundException(string wallet) : base("Wallet '" + wallet + "' not found!")
        {
            WalletNumber = wallet;
        }

        public WalletNotFoundException(string wallet, Exception inner) : base("Wallet '" + wallet + "' not found!", inner)
        {
            WalletNumber = wallet;
        }

        protected WalletNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            WalletNumber = "";
        }
    }
}
