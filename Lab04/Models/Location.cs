using System;
using System.Globalization;

namespace Lab04.Models
{
    public class Location
    {
        public double X {get;}
        public double Y {get;}
        public string Name {get;}
        public CultureInfo Culture {get;}
    public Location(double _x, double _y, string _name, CultureInfo _culture)
    {
        X = _x;
        Y = _y;
        Name = _name;
        Culture = _culture;
    }

    public static (double X, double Y) operator -(Location l1, Location l2)
    {
        double deltaX = l1.X - l2.X;
        double deltaY = l1.Y - l2.Y;
        return (deltaX, deltaY);
    }
    public override string ToString()
        {

            string formattedX = X.ToString("F4", Culture).PadLeft(12);
            string formattedY = Y.ToString("F4", Culture).PadLeft(12);
            return $"[{Name}] at ([{formattedX}],[{formattedY}]).";
        }
    }
}   
