using System;
using System.IO;
using System.Text;

namespace Archivers
{
    public class FileArchiver : IFileArchiver
    {
        private readonly IArchiver archiver;

        public FileArchiver(IArchiver archiver)
        {
            this.archiver = archiver;
        }

        public void CompressFile(string filename, string fileOutName)
        {
            var ifStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var ofStream = new FileStream(fileOutName, FileMode.Create, FileAccess.ReadWrite);

            var array = new byte[ifStream.Length];
            ifStream.Read(array, 0, array.Length);
            var entry = Encoding.Default.GetString(array);
            
            var compressed = archiver.Compress(entry);

            ofStream.Write(compressed, 0, compressed.Length);

            var originalSize = ifStream.Length;
            var compressedSize = ofStream.Length;
            ifStream.Close();
            ofStream.Close();

            var coefficient = (double)originalSize / compressedSize;
            Console.WriteLine($"Размер файла ДО сжатия: {originalSize}");
            Console.WriteLine($"Размер файла ПОСЛЕ сжатия: {compressedSize}");
            Console.WriteLine($"Коэффициент сжатия: {coefficient}");
        }

        public void DecompressFile(string filename, string fileOutName)
        {
            var ifStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var ofStream = new FileStream(fileOutName, FileMode.Create, FileAccess.ReadWrite);

            var array = new byte[ifStream.Length];
            ifStream.Read(array, 0, array.Length);
            
            var decompressed = archiver.Decompress(array);
            var decompressedAsBytes = Encoding.Default.GetBytes(decompressed);

            ofStream.Write(decompressedAsBytes, 0, decompressedAsBytes.Length);
            
            ifStream.Close();
            ofStream.Close();
        }
    }
}