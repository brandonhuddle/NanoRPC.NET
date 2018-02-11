using System.Numerics;

namespace NanoRpc
{
    public class PendingTransaction
    {
        public string Hash { get; set; }
        public BigInteger Amount { get; set; }
        public string Source { get; set; }
    }
}
