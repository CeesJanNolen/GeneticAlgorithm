using System;

namespace GeneticAlgorithm
{
    public static class Converter
    {
        public static int ToBinary(this string value)
        {
            var result = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var digit = Convert.ToInt32("" + value[i]);
                result += digit * (int) Math.Pow(2, (value.Length - 1) - i);
            }
            return result;
        }

        public static int ToBinary(this int[] array)
        {
            var result = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var digit = Convert.ToInt32("" + array[i]);
                result += digit * (int) Math.Pow(2, array.Length - 1 - i);
            }
            return result;
        }

        public static int BinaryConvert(this string binary, int _base)
        {
            var result = 0;
            for (var i = 0; i < binary.Length; i++)
            {
                var digit = Convert.ToInt32("" + binary[i]);
                result += digit * (int) Math.Pow(_base, binary.Length - 1 - i);
            }
            return result;
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