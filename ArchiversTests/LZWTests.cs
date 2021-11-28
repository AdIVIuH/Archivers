using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Archivers;
using Archivers.Huffman;
using Archivers.Huffman2;
using NUnit.Framework;

namespace ArchiversTests
{
    public class LZWTests
    {
        private IArchiver archiever;
        private IFileArchiver fileArchiver;
        private string originalDirPath;
        private string compressedDirPath;
        private string decompressedDirPath;

        [SetUp]
        public void Setup()
        {
            archiever = new LZW();
            fileArchiver = new FileArchiver(archiever);
            originalDirPath = "../../../Texts/Originals/";
            compressedDirPath = "../../../Texts/Compressed/";
            decompressedDirPath = "../../../Texts/Decompressed/";
        }

        [TestCase("small.txt")]
        [TestCase("medium.txt")]
        [TestCase("big.txt")]
        [TestCase("huge.txt")]
        public void RunTest(string fileName)
        {
            var originalPath = originalDirPath + fileName;
            var compressedPath = compressedDirPath + fileName;
            var decompressedPath = decompressedDirPath + fileName;
            OriginalAndDecompressedAreEquail(originalPath, compressedPath, decompressedPath);
        }

        private void OriginalAndDecompressedAreEquail(string originalPath, string compressedPath,
            string decompressedPath)
        {
            fileArchiver.CompressFile(originalPath, compressedPath);
            fileArchiver.DecompressFile(compressedPath, decompressedPath);
            var areEquals = File.ReadLines(originalPath)
                .SequenceEqual(File.ReadLines(decompressedPath));
            Assert.IsTrue(areEquals);
        }
    }
}