using System;
using Archivers.Huffman2;

namespace Archivers.Huffman
{
    public class Huffman : IArchiver
    {
        public HuffmanTree HuffmanTree { get; set; }
        
        public byte[] Compress(string input)
        {
            HuffmanTree = new HuffmanTree(input);
            var compressed = HuffmanTree.Encode(input);
            
            var size = (int)Math.Ceiling((decimal)compressed.Count / 8);
            var compressedAsBytes = new byte[size];
            compressed.CopyTo(compressedAsBytes, 0);
            
            return compressedAsBytes;
        }

        public string Decompress(byte[] input)
        {
            return HuffmanTree.Decode(input);
        }
    }
}