namespace YourNamespaceName
{
    public struct WorldPosition
    {
        public double Longitude;
        public double Latitude;
    }

    public abstract class Vehicle
    {
        public float totalDistanceCovered = 0;
        public string Name { get; private set; }
        public abstract void Move(float distance);
        public Vehicle(string name)
        {
            Name = name;
        }
    }

    public class Car : Vehicle
    {
        public string Make { get; }
        public string Model { get; set; }
        private float _currentFuelAmount = 0;
        public const int MaxFuelCapacity = 50;
        private static int _totalCarsCreated = 0;
        public readonly string VIN;
        public const float FuelConsumptionRatio = 12;
        public int ID;

        public override void Move(float distance)
        {
            float maxDistance = CurrentFuelAmount / FuelConsumptionRatio * 100;
            float distanceCovered = Math.Min(maxDistance, distance);
            totalDistanceCovered += distanceCovered;
            CurrentFuelAmount -= distanceCovered * FuelConsumptionRatio / 100;
        }

        public Car(string name, string make, string model) : base(name)
        {
            this.Make = make;
            this.Model = model;
            this.ID = ++_totalCarsCreated;
        }
        public Car(string make, string model) : this(make, model, $"{make} {model}")
        {
        }


        public float CurrentFuelAmount
        {
            get => _currentFuelAmount;
            set
            {
                if (value > 0)
                    _currentFuelAmount = value;
            }
        }
        public void Refuel(int fuel)
        {
            _currentFuelAmount += fuel;
        }
        public float GetCurrentFuel() => _currentFuelAmount;

        public static bool CompareCars(Car car1, Car car2)
        {
            return car1.Make == car2.Make && car1.Model == car2.Model;
        }
    }
    public class Bike : Vehicle
    {
        public Bike() : base("Bike") { }
        public override void Move(float distance)
        {
            totalDistanceCovered += distance;
        }

    }


    public class Logger
    {
        public static string LogFilePath;
        static Logger()
        {
            LogFilePath = "default_log.txt";
            Console.WriteLine("Initializing Logger.");
        }

        public static void Log(string message)
        {
            Console.WriteLine($"Logging message to {LogFilePath}: {message}");
        }
    }
    public static class CarFactory
    {
        public static Car CreateHondaCivic()
        {
            var car = new Car("Honda", "Civic");
            car.Refuel(10);
            return car;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Application started.");
            // Value Type (struct) behavior
            WorldPosition p1 = new WorldPosition { Longitude = 21.02, Latitude = 52.24 };
            WorldPosition p2 = p1;
            p2.Longitude = 140.5; 
            Console.WriteLine("p1 Longitude: {0}, p2 Longitude: {1}", p1.Longitude,
            p2.Longitude);

            Car car1 = new Car("Toyota", "Camry");
            Car car2 = car1;
            Car car3 = new Car("Skoda", "Octavia");
            Car toyota1 = new Car("Toyota", "Camry");
            Car toyota2 = new Car("Toyota", "Camry");
            Console.WriteLine("Is toyota1 the same car as toyota2? {0}",
                Car.CompareCars(toyota1 , toyota2)? "Yes": "no");    

            car3.Refuel(60);
            car2.Model = "Corolla"; 
            Console.WriteLine("Car1 model: {0}, Car2 model: {1}", car1.Model, car2.Model);
            Console.WriteLine("Car1 fuel amount: {0}", car1.GetCurrentFuel());
            Console.WriteLine("Car3 fuel amount: {0}", car3.GetCurrentFuel());

            Console.WriteLine("car1 ID: {0}", car1.ID);
            Console.WriteLine("car2 ID: {0}", car2.ID);
            Console.WriteLine("car3 ID: {0}", car3.ID);
            Console.WriteLine("toyota1 ID: {0}", toyota1.ID);
            Console.WriteLine("toyota2 ID: {0}", toyota2.ID);

            Car hondaCivic = CarFactory.CreateHondaCivic();

            Car tesla = new Car("Tesla", "T");
            Console.WriteLine("tesla TotalDistance: {0}", tesla.totalDistanceCovered);
            tesla.Move(100);
            Console.WriteLine("tesla TotalDistance (after Move): {0}", tesla.totalDistanceCovered);

            Bike bike = new Bike();
            Console.WriteLine("bike TotalDistance: {0}", bike.totalDistanceCovered);
            bike.Move(10);
            Console.WriteLine("bike TotalDistance (after Move): {0}",
            bike.totalDistanceCovered);

            Car kia = new Car("Kia", "Sportage");
            kia.Refuel(6);
            Console.WriteLine("kia TotalDistance: {0}", kia.totalDistanceCovered);
            kia.Move(100); 
            Console.WriteLine("kia TotalDistance (after Move): {0}",
            kia.totalDistanceCovered);
            Bike bike2 = new Bike();
            Console.WriteLine("bike2 TotalDistance: {0}", bike2.totalDistanceCovered);
            bike2.Move(20); // Moved 20 km by bike
            Console.WriteLine("bike2 TotalDistance (after Move): {0}",
            bike2.totalDistanceCovered);

            Console.WriteLine("array:");
            Vehicle[] vehicles = new Vehicle[] {
             CarFactory.CreateHondaCivic(),
             new Bike()
            };
            foreach (var vehicle in vehicles)
            {
                vehicle.Move(100); // Calls the appropriate Move method for Car or Bike
            }
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine("{0} TotalDistance: {1}", vehicle.Name,
               vehicle.totalDistanceCovered);
            }

        }
    }
}
