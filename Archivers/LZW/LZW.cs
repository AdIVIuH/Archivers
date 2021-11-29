using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Archivers
{
    public class LZW : IArchiver
    {
        public byte[] Compress(string input)
        {
            if (input.Length == 0)
                return Array.Empty<byte>();
            var dictionary = GetInitialDictionaryBySymbol();

            var current = string.Empty;
            var compressed = new List<short>();

            foreach (var ch in input)
            {
                var workingString = current + ch;
                if (dictionary.ContainsKey(workingString))
                    current = workingString;
                else
                {
                    compressed.Add(dictionary[current]);
                    dictionary.Add(workingString, (short)dictionary.Count);
                    current = ch.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(current))
                compressed.Add(dictionary[current]);
            var result = compressed.SelectMany(BitConverter.GetBytes).ToArray();
            return result;
        }

        public string Decompress(byte[] compressed)
        {
            if (compressed.Length == 0)
                return string.Empty;

            var indexes = GetIndexies(compressed);
            var dict = GetInitialDictionaryByIndex();

            var workingString = dict[indexes[0]];
            indexes.RemoveAt(0);

            var decompressed = new StringBuilder(workingString);

            foreach (var index in indexes)
            {
                string entry = null;
                if (dict.ContainsKey(index))
                    entry = dict[index];
                else if (index == dict.Count)
                    entry = workingString + workingString[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dict.Add((short)dict.Count, workingString + entry?.FirstOrDefault());

                workingString = entry;
            }

            return decompressed.ToString();
        }

        private static List<short> GetIndexies(byte[] compressed)
        {
            var indexes = new List<short>();
            var buffer = new List<byte>();
            foreach (var b in compressed)
            {
                buffer.Add(b);
                if (buffer.Count == 2)
                {
                    indexes.Add(BitConverter.ToInt16(buffer.ToArray(), 0));
                    buffer.Clear();
                }
            }

            return indexes;
        }

        private static Dictionary<string, short> GetInitialDictionaryBySymbol()
        {
            var dict = new Dictionary<string, short>();
            for (short i = 0; i < 256; i++)
                dict.Add(((char)i).ToString(), i);
            return dict;
        }

        private static Dictionary<short, string> GetInitialDictionaryByIndex()
        {
            var dict = new Dictionary<short, string>();
            for (short i = 0; i < 256; i++)
                dict.Add(i, ((char)i).ToString());
            return dict;
        }
    }
}