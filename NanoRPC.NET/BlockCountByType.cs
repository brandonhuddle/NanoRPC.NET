using System;
using System.Runtime.Serialization;

namespace NanoRpc
{
    public class BlockCountByType
    {
        [DataMember(Name = "send")]
        public UInt64 Send { get; set; }
        [DataMember(Name = "receive")]
        public UInt64 Receive { get; set; }
        [DataMember(Name = "open")]
        public UInt64 Open { get; set; }
        [DataMember(Name = "change")]
        public UInt64 Change { get; set; }
    }
}
