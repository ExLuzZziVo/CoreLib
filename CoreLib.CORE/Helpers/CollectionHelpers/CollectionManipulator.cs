namespace CoreLib.CORE.Helpers.CollectionHelpers
{
    public static class CollectionManipulator
    {
        /// <summary>
        /// Creates an array from a range of digits
        /// </summary>
        /// <param name="startValue">Start of range</param>
        /// <param name="endValue">End of range</param>
        /// <returns>Array from a range of digits</returns>
        public static int[] CreateDigitRangeArray(int startValue, int endValue)
        {
            if (startValue > endValue)
            {
                return CreateDigitRangeArray(endValue, startValue);
            }

            if (startValue == endValue)
            {
                return [startValue];
            }

            var length = endValue - startValue + 1;
            var result = new int[length];

            for (int index = 0, value = startValue; index < length; index++, value++)
            {
                result[index] = value;
            }

            return result;
        }
    }
}
