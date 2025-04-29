using System;

namespace Project.Content
{
    public static class ObjectCountHelper
    {
        /// <summary>
        /// Makes sure the count is divisible by the given number.
        /// Adds extra if needed.
        /// </summary>
        public static int AdjustToFit(int initialCount, int prefabCount)
        {
            if (prefabCount <= 0)
                throw new ArgumentException("prefabCount must be greater than 0");

            if (initialCount % prefabCount == 0)
            {
                return initialCount;
            }
            else
            {
                int remainder = initialCount % prefabCount;
                int adjustment = prefabCount - remainder;
                return initialCount + adjustment;
            }
        }
    }
}
