namespace CoreLayer.Common
{
    using System;

    public static class StringExtensions
    {
        public static int GetLength(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            int result = input.Length;

            return result;
        }
    }
}
