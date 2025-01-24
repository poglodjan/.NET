

namespace UnsafeWorkshop
{
    internal static class MathOperations
    {
        public static unsafe float UnsafeSum(float[] array)
        {
            float sum = 0;
            fixed(float* pointer = array)
            {
                float* current = pointer;
                for(int i = 0; i<array.Length; i++)
                {
                    sum += *current;
                    current++;
                }
            }
            return sum;
        }

        public static float Sum(float[] array)
        {
            float sum = 0;
            foreach (var item in array)
            {
                sum += item;
            }
            return sum;
        }

        public static float Clamp(float value, float min = 0.0f, float max = 1.0f)
        {
            return Math.Min(Math.Max(value, min), max); ;
        }
    }
}
