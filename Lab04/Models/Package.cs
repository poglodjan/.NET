using System;
using System.Diagnostics;

namespace Lab04.Models
{
    public class Package
    {
        public readonly double Size;
        public readonly double Weight;
        public double Volume => Size*Size*Size; // or // public double Volume{get{ return Size*Size*Size; }}
        public Customer? Sender {get; set;}
        public Customer? Recipent {get; set;}
        public Location? Source {get; set;}
        public Location? Destination {get; set;}
        public DateTime ShippedAt {get; set;}
        public DateTime? DeliveredAt {get; set;}
        public Priority Priority {get; set;} = Priority.Standard;
        
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
        // cost:
        public double? Cost
        {
            get
            {
            if (Source == null || Destination == null)
                return null;
            double distance = Math.Sqrt(Math.Pow(Destination.X - Source.X,2) + Math.Pow(Destination.Y - Source.Y,2));
            double multiplyer = Priority switch
            {
                Priority.Standard | Priority.Fragile => 100,
                Priority.Express | Priority.Fragile => 200,
                _ => 50
            };
            return distance * multiplyer > 0? (double?) distance * multiplyer : null;
            }
        }
        public double? DeliverySpeed
        {
            get
            {
                if (Source == null || Destination == null || DeliveredAt == null || ShippedAt >= DeliveredAt)
                    return null;
                double distance = Math.Sqrt(Math.Pow(Destination.X - Source.X, 2) + Math.Pow(Destination.Y - Source.Y, 2));
                double hours = (DeliveredAt.Value - ShippedAt).TotalHours;
                return hours > 0 ? (double?) distance / hours : null;
            }
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