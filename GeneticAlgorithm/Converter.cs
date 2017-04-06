using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public static class Converter
    {
        public static int ToBinary(this string value)
        {
            return value.Select(Convert.ToInt32)
                .Select((digit, i) => digit * (int) Math.Pow(2, value.Length - 1 - i))
                .Sum();
        }

        public static int ToBinary(this int[] array)
        {
            return array.Select((item, index) => item * (int) Math.Pow(2, array.Length - 1 - index)).Sum();
        }

        public static int BinaryConvert(this string binary, int _base)
        {
            return binary.Select(Convert.ToInt32)
                .Select((digit, i) => digit * (int) Math.Pow(_base, binary.Length - 1 - i))
                .Sum();
        }

        public static int[] ToBitsArray(this int value, int size)
        {
            var bits = new int[size];

            for (var i = size - 1; i >= 0; i--)
            {
                var bit = value - (int) Math.Pow(2, i);
                bits[Math.Abs(i + 1 - size)] = bit >= 0 ? 1 : 0;
                value = bit >= 0 ? bit : value;
            }
            return bits;
        }

        public static int[] Merge(this int[] array, int[] array2)
        {
            var newarray = new int[array.Length + array2.Length];
            array.CopyTo(newarray, 0);
            array2.CopyTo(newarray, array.Length);
            return newarray;
        }
    }
}