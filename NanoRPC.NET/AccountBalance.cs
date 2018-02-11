using System.Numerics;
using System.Runtime.Serialization;

namespace NanoRpc
{
    public class AccountBalance
    {
        public string Account { get; set; }
        [DataMember(Name = "balance")]
        public BigInteger Balance { get; set; }
        [DataMember(Name = "pending")]
        public BigInteger Pending { get; set; }
    }
}
