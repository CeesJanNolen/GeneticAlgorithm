using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Binary
    {
        int[] _binary;
        public int Size;

        public Binary(int[] binary)
        {
            Size = binary.Length;
            _binary = binary;
        }

        public Binary(int size, int value)
        {
            Size = size;
            _binary = value.ToBitsArray(size);
        }

        public int ToInt()
        {
            return _binary.ToBinary();
        }

        public int[] GetPart(int start, int end)
        {
            var result = new List<int>();
            for (var i = start; i < end; i++)
            {
                result.Add(_binary[i]);
            }
            return result.ToArray();
        }

        public void Switch(int position)
        {
            _binary[position] = _binary[position] == 1 ? 0 : 1;
        }

        public override string ToString()
        {
            return _binary.Aggregate("", (currentstring, index) => currentstring + index.ToString());
        }
    }
}