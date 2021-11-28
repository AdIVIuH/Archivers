namespace Archivers
{
    public interface IArchiver
    {
        byte[] Compress(string input);
        string Decompress(byte[] input);
    }
}
