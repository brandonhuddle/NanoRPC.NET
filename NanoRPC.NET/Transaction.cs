using System.Numerics;

namespace NanoRpc
{
    public class Transaction
    {
        public string Hash { get; set; }
        // TODO: Make enum
        public string Type { get; set; }
        public string Account { get; set; }
        public BigInteger Amount { get; set; }
    }
}
