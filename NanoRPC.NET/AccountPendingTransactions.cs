namespace NanoRpc
{
    public class AccountPendingTransactions
    {
        public string Account { get; set; }
        public PendingTransaction[] PendingTransactions { get; set; }
    }
}
