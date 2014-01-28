using System;

namespace GU.MZ.BL.Tests
{
    public static class RandomProvider
    {
        private static readonly Random _random = new Random();

        public static string RandomNumberString(int length)
        {
            int authCodeLen = length;
            const string chars = "123456789";

            char[] buffer = new char[authCodeLen];

            for (int i = 0; i < authCodeLen; i++)
                buffer[i] = chars[_random.Next(chars.Length)];

            return new string(buffer);
        }

        public static int Random(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static int Random(int minValue)
        {
            return _random.Next(minValue);
        }
    }
}
