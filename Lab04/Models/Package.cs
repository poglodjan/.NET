using System;

namespace Lab04.Models
{
    public class Package
    {
        public readonly double Size;
        public readonly double Weight;
        public double Volume
        {
            get{ return Size*Size*Size; }
        }
        // or // public double Volume => Size*Size*Size;
        public Package(double _size, double _weight)
        {
            Size = _size;
            Weight = _weight;
        }
        public static Package operator +(Package p1, Package p2)
        {
            double p3Weight = p1.Weight + p2.Weight;
            double p3Size = p1.Size + p2.Size;

            return new Package(p3Size, p3Weight);
        }
        public static bool operator ==(Package p1, Package p2)
        {
            if (ReferenceEquals(p1, p2)) return true;
            return p1.Size == p2.Size && p1.Weight == p2.Weight;
        }

        public static bool operator !=(Package p1, Package p2)
        {
            return !(p1 == p2);
        }
        // tuple in/out:        
        public void Deconstruct(out double size, out double weight)
        {
            weight = Weight;
            size = Size;
        }
        public static explicit operator Package((double size, double weight) tuple)
        {
            return new Package(tuple.size, tuple.weight);
        }
        // methods for == !=
        public override bool Equals(object? obj)
        {
            if(obj is Package other)
            {
                return this == other;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Size, Weight);
        }
        //
    }
}