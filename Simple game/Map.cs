using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    public class Map
    {
        public Tile[,] Data { get; private set; }
        public List<Room> Rooms { get; private set; }
        public Dictionary<Point, string> Items { get; private set; }

        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1);

        public Map(IList<Room> rooms, IDictionary<Point, string> items)
        {
            // Kopiowanie danych wejściowych
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            Rooms = rooms.ToList();
            Items = items.ToDictionary(item => item.Key, item => item.Value);

            foreach (var room in rooms)
            {
                minX = Math.Min(minX, room.TopLeft.X);
                minY = Math.Min(minY, room.TopLeft.Y);
                maxX = Math.Max(maxX, room.BottomRight.X);
                maxY = Math.Max(maxY, room.BottomRight.Y);
            }

            int mapWidth = maxX - minX + 1;
            int mapHeight = maxY - minY + 1;
            Data = new Tile[mapWidth, mapHeight];

            // Wypełnianie mapy podłogą
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Data[x, y] = Tile.Floor;
                }
            }

            // Rysowanie pokoi (ściany i drzwi)
            foreach (var room in rooms)
            {
                for (int x = room.TopLeft.X; x <= room.BottomRight.X; x++)
                {
                    for (int y = room.TopLeft.Y; y <= room.BottomRight.Y; y++)
                    {
                        Data[x, y] = Tile.Wall;
                        if (x == room.Door.X && y == room.Door.Y)
                            Data[x, y] = Tile.Door;
                    }
                }
            }
        }

        public List<string> Search(Point position)
        {
            // Znajdź pokój, w którym znajduje się podana pozycja
            var room = Rooms.FirstOrDefault(r =>
                position.X >= r.TopLeft.X && position.X <= r.BottomRight.X &&
                position.Y >= r.TopLeft.Y && position.Y <= r.BottomRight.Y);

            // Jeśli pokój nie istnieje, zwróć pustą listę
            if (room.Equals(default(Room)))
                return new List<string>();

            // Znajdź wszystkie przedmioty w tym pokoju
            return Items
                .Where(item =>
                    item.Key.X >= room.TopLeft.X && item.Key.X <= room.BottomRight.X &&
                    item.Key.Y >= room.TopLeft.Y && item.Key.Y <= room.BottomRight.Y)
                .Select(item => item.Value)
                .ToList();
        }

        public string Interact(Point position)
        {
            // Sprawdź, czy na danej pozycji znajduje się przedmiot
            if (Items.ContainsKey(position))
            {
                string item = Items[position];
                Items.Remove(position); // Usuń przedmiot
                return item;
            }

            return string.Empty; // Zwróć pusty string, jeśli brak przedmiotu
        }

        public bool InBounds(Point point)
        {
            return point.X >= 0 && point.Y >= 0 &&
                   point.X < Data.GetLength(0) &&
                   point.Y < Data.GetLength(1);
        }
    }
}