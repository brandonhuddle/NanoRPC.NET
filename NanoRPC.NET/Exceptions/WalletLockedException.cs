using System;
using System.Runtime.Serialization;

namespace NanoRpc.Exceptions
{
    [Serializable]
    public class WalletLockedException : Exception
    {
        public string WalletNumber { get; private set; }

        public WalletLockedException() : base("Wallet is locked!")
        {
            WalletNumber = "";
        }

        public WalletLockedException(string wallet) : base("Wallet '" + wallet + "' is locked!")
        {
            WalletNumber = wallet;
        }

        public WalletLockedException(string wallet, Exception inner) : base("Wallet '" + wallet + "' is locked!", inner)
        {
            WalletNumber = wallet;
        }

        protected WalletLockedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            WalletNumber = "";
        }
    }
}
