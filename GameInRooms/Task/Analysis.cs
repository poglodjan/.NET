using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    public static class Analysis
    {
        public enum Quarter
        {
            TopLeft,
            TopRight, 
            BottomLeft,
            BottomRight
        }

        static Quarter WhichQuarter(Point p, int mapWidth, int mapHeight)
        {
            Point center = new Point(mapWidth / 2, mapHeight / 2);

            if (p.X < center.X && p.Y < center.Y) return Quarter.TopLeft;
            else if (p.X >= center.X && p.Y < center.Y) return Quarter.TopRight;
            else if (p.X < center.X && p.Y >= center.Y) return Quarter.BottomLeft;
            else return Quarter.BottomRight;
        }

        public static Dictionary<Quarter, int> CountItemsOnQuarter(IEnumerable<Map> maps)
        {
            return maps
                .SelectMany(map => map.Items, (map, item) => new
                {
                    Quarter = WhichQuarter(item.Key, map.Width, map.Height),
                    Item = item.Value
                }) // Rozwijamy wszystkie przedmioty wraz z ich ćwiartkami
                .GroupBy(entry => entry.Quarter) // Grupowanie według ćwiartek
                .ToDictionary(
                    group => group.Key,          // Klucz to ćwiartka
                    group => group.Count()       // Wartość to liczba przedmiotów w ćwiartce
                );
        }
    }
}
