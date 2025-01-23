namespace Jabba.Services.Hash
{
    public interface IHashService
    {
        public string Hash(string input);
        public string Hash(byte[] input);
        public string Hash(ReadOnlySpan<byte> input);
        public string Hash(Span<byte> input);
        public string Hash(ReadOnlyMemory<byte> input);
        public string Hash(Memory<byte> input);
    }
}
