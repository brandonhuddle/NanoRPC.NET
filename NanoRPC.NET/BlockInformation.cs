using System.Numerics;

namespace NanoRpc
{
    public class BlockInformation
    {
        public string BlockHash { get; set; }
        public string BlockAccount { get; set; }
        public BigInteger Amount { get; set; }
        public string Contents { get; set; }
        public bool Pending { get; set; }
        public string SourceAccount { get; set; }
    }
}
