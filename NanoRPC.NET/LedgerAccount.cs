using System;
using System.Numerics;

namespace NanoRpc
{
    public class LedgerAccount
    {
        public string Account { get; set; }
        public string Frontier { get; set; }
        public string OpenBlock { get; set; }
        public BigInteger Balance { get; set; }
        public string RepresentativeBlock { get; set; }
        public UInt64 ModifiedTimestamp { get; set; }
        public UInt64 BlockCount { get; set; }
        public string Representative { get; set; }
        public BigInteger Weight { get; set; }
        public BigInteger Pending { get; set; }
    }
}
