namespace Archivers.Huffman2
{
    public interface IFileArchiver
    {
        void CompressFile(string filename, string fileOutName);
        void DecompressFile(string filename, string fileOutName);
    }
}