
using System.Runtime.InteropServices;

namespace UnsafeWorkshop
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct UnsafeColor
    {
        public const int MaxComponents = 4;
        [FieldOffset(0)]public readonly float R = 0;
        [FieldOffset(4)]public readonly float G = 0;
        [FieldOffset(8)]public readonly float B = 0;
        [FieldOffset(12)]public readonly float A = 0;

        // Assume that colorComponents is fixed
        public UnsafeColor(float* colorComponents, int size)
        {
            if(colorComponents == null)
                throw new ArgumentNullException(nameof(colorComponents));
            if (size > MaxComponents)
            {
                throw new ArgumentException("To much data in input");
            }

            fixed (float* ptr = &R)
            {
                for (int i = 0; i < size; i++)
                {
                    ptr[i] = MathOperations.Clamp(colorComponents[i]);
                }

                for (int i = size; i < MaxComponents; i++)
                {
                    ptr[i] = 0;
                }
            }
        }

        public static UnsafeColor Averaged(UnsafeColor left, UnsafeColor right)
        {
            float* components = stackalloc float[4];

            for (int i = 0; i < 4; i++)
            {
                components[i] = ((&left.R)[i] + (&right.R)[i]) * 0.5f;
            }

            return new UnsafeColor(components, 4); 
        }

        public static UnsafeColor Negative(UnsafeColor color)
        {
            float[] components = new float[4];

            for (int i = 0; i < 4; i++)
            {
                components[i] = 1 - (&color.R)[i];
            }

            UnsafeColor result;
            fixed (float* ptr = components)
            {
                result =  new UnsafeColor(ptr, 4);
            }

            return result;
        }

        public static UnsafeColor operator *(UnsafeColor color, float scalar)
        {
            float* components = stackalloc float[4];

            for (int i = 0; i < UnsafeColor.MaxComponents; i++)
            {
                components[i] = (&color.R)[i] * scalar;
            }

            return new UnsafeColor(components, UnsafeColor.MaxComponents);
        }

        public override string ToString()
        {
            return string.Format("R: {0:0.000}, G: {1:0.000}, B: {2:0.000}, A: {3:0.000}", R, G, B, A);
        }
    }
}