using System.Security.Cryptography;
using System.Text;

namespace Jabba.Services.Hash;

public class Md5HashService : IHashService
{
    public string Hash(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);

        return Hash(bytes);
    }

    public string Hash(byte[] input)
    {
        return Hash(input.AsMemory());
    }

    public string Hash(ReadOnlySpan<byte> input)
    {
        return Hash(input.ToArray());
    }

    public string Hash(Span<byte> input)
    {
        return Hash(input.ToArray());
    }

    public string Hash(ReadOnlyMemory<byte> input)
    {
        MemoryTypeStream stream = new MemoryTypeStream(input);

        return Convert.ToHexString(MD5.HashData(stream)).Replace("-", "");
    }

    public string Hash(Memory<byte> input)
    {
        return ReadonlyMemoryHash(input);
    }

    private string ReadonlyMemoryHash(ReadOnlyMemory<byte> input)
    {
        return Hash(input);
    }
}
