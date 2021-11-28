using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivers.Huffman2
{
    public class HuffmanTree
    {
        private readonly List<Node> nodes = new();
        public Node Root { get; set; }
        private readonly Dictionary<char, int> frequencies = new();
        private int countBits = 0;
        public HuffmanTree(string source)
        {
            Build(source);
        }

        private void Build(string source)
        {
            foreach (var t in source)
            {
                if (!frequencies.ContainsKey(t))
                    frequencies.Add(t, 0);

                frequencies[t]++;
            }

            foreach (var (ch, freq) in frequencies)
                nodes.Add(new Node { Symbol = ch, Frequency = freq });

            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    var taken = orderedNodes.Take(2).ToList();

                    // Create a parent node by combining the frequencies
                    var parent = new Node
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(string source)
        {
            var encodedSource = new List<bool>();

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var ch in source)
            {
                var encodedSymbol = Root.Traverse(ch, new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }
            
            // 1010101010101100101
            var bits = new BitArray(encodedSource.ToArray());
            countBits = bits.Count;
            
            return bits;
        }

        public string Decode(string strBits)
        {
            var data = Encoding.Default.GetBytes(strBits);
            return Decode(data);
        }

        public string Decode(byte[] bytes)
        {
            var bitArray = new BitArray(bytes);
            
            return Decode(bitArray);
        }

        public string Decode(BitArray bits)
        {
            var current = Root;
            var decoded = new StringBuilder();

            for (var i = 0; i < countBits; i++)
            {
                if (bits[i])
                {
                    if (current.Right != null)
                        current = current.Right;
                }
                else
                {
                    if (current.Left != null)
                        current = current.Left;
                }

                // ReSharper disable once InvertIf
                if (IsLeaf(current))
                {
                    decoded.Append(current.Symbol);
                    current = Root;
                }
            }

            return decoded.ToString();
        }

        private static bool IsLeaf(Node node)
        {
            return node.Left == null && node.Right == null;
        }
    }
}