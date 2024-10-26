using Lab04.Models;
using System.Globalization;
using System.Collections.Generic;
using Lab04.Data;
using System.Linq;

namespace Lab04.Services
{
    public class PackageManager
    {
        private List<Package> _packages = new List<Package>();
        private Random _random = new Random(12345);
        public Package CreatePackage()
        {
            // random double from [10, 100)
            double size = _random.NextDouble() * 90 + 10;
            double weight = _random.NextDouble() * 90 + 10;
            DateTime shippedAt = DateTime.Now.AddDays(-_random.Next(1, 10));
            DateTime? deliveredAt = null;
            if(_random.Next(0,100) < 75)
            {
                deliveredAt = shippedAt.AddDays(_random.Next(1, 10));
            }
            Customer sender = Repository.DrawCustomer(_random);
            Customer recipent = Repository.DrawCustomer(_random);
            Location source = Repository.DrawLocation(_random);
            Location destination = Repository.DrawLocation(_random);
            Package newPackage = new Package(size, weight)
            {
                Sender = sender,
                Recipent = recipent,
                Source = source,
                Destination = destination,
                ShippedAt = shippedAt,
                DeliveredAt = deliveredAt
            };
            _packages.Add(newPackage);
            return newPackage;
        }
        public void MakeReport()
        {
            foreach(Package pack in _packages)
            {
                string sourceName = pack.Source?.Name ?? "Uknown";
                string destinationName = pack.Destination?.Name ?? "Uknown";
                string shippedDate = pack.ShippedAt.ToString("D", pack.Source?.Culture ?? CultureInfo.InvariantCulture);
                string deliveryInfo = pack.DeliveredAt.HasValue ?
                 pack.DeliveredAt.Value.ToString("D", pack.Destination?.Culture ?? CultureInfo.InvariantCulture) 
                 : "not delivered yet";
                Console.WriteLine($"{sourceName} (at {shippedDate}) => {destinationName} (at {deliveryInfo})");
            }
        }

        // enumerables:
        public Package[] this[Range range]
        {
            get
            {
                int start = range.Start.IsFromEnd ? _packages.Count - range.Start.Value : range.Start.Value;
                int end = range.End.IsFromEnd ? _packages.Count - range.End.Value : range.End.Value;

                List<Package> result = new List<Package>();
                for (int i = start; i < end; i++)
                {
                    result.Add(_packages[i]);
                }
                return result.ToArray();
            }
        }
        public Package[] this[DateTime from, DateTime to]
        {
            get
            {
                List<Package> result = new List<Package>();
                foreach (var package in _packages)
                {
                    if (package.ShippedAt >= from && package.DeliveredAt.HasValue && package.DeliveredAt.Value <= to)
                    {
                        result.Add(package);
                    }
                }
                return result.ToArray();
            }
        }
    }
}