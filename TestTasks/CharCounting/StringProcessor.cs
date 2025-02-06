using System;
using System.Collections.Generic;

namespace TestTasks.CharCounting
{
    using System.Linq;

    public class StringProcessor
    {
        private static readonly int ChunkSize = 5 * 1024 * 1024;

        public (char symbol, int count)[] GetCharCount(string veryLongString, char[] countedChars)
        {
            if (countedChars == null || countedChars.Length == 0 || string.IsNullOrEmpty(veryLongString))
            {
                return Array.Empty<(char symbol, int count)>();
            }

            var length = veryLongString.Length;
            var distinctChars = new HashSet<char>(countedChars);
            var rawResult = new Dictionary<char, int>(distinctChars.ToDictionary(i => i, _=> 0));

            var stringReadOnlyMemory = veryLongString.AsMemory();

            foreach (var chunk in GetSplit(stringReadOnlyMemory, length))
            {
                foreach (var cahr in chunk.Span)
                {
                    if (rawResult.TryGetValue(cahr, out int count))
                    {
                        rawResult[cahr] = count + 1;
                    }
                }
            }

            return rawResult.Select(kv => (kv.Key, kv.Value)).ToArray();
        }

        private IEnumerable<ReadOnlyMemory<char>> GetSplit(ReadOnlyMemory<char> stringReadOnlyMemory, int length)
        {
            for (int i = 0; i <= length; i += ChunkSize)
            {
                yield return stringReadOnlyMemory.Slice(i, Math.Min(ChunkSize, length - i));
            }
        }
    }
}
