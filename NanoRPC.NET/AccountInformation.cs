using System;

namespace NanoRpc
{
    public class AccountInformation
    {
        public string Account { get; set; }
        // A.k.a. head
        public string Frontier { get; set; }
        public string OpenBlock { get; set; }
        public string RepresentativeBlock { get; set; }
        public string Balance { get; set; }
        public UInt64 ModifiedTimestamp { get; set; }
        public UInt64 BlockCount { get; set; }
        // Optional
        public string Representative { get; set; }
        // Optional
        public string Weight { get; set; }
        // Optional, a.k.a. account pending 
        public string Pending { get; set; }
    }
}
