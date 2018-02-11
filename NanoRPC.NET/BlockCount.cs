using System;
using System.Runtime.Serialization;

namespace NanoRpc
{
    public class BlockCount
    {
        [DataMember(Name = "count")]
        public UInt64 Count { get; set; }
        [DataMember(Name = "unchecked")]
        public UInt64 Unchecked { get; set; }
    }
}
