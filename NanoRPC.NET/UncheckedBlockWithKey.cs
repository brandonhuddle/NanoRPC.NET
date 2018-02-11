namespace NanoRpc
{
    public class UncheckedBlockWithKey
    {
        public string Key { get; set; }
        public string Hash { get; set; }
        // Json of the block, not the hash
        public string Block { get; set; }
    }
}
