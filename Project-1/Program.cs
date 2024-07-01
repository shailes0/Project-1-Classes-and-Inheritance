using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApplianceManagement
{
    abstract class Appliance
    {
        public string ItemNumber { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public int Wattage { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }

        public Appliance(string itemNumber, string brand, int quantity, int wattage, string color, double price)
        {
            ItemNumber = itemNumber;
            Brand = brand;
            Quantity = quantity;
            Wattage = wattage;
            Color = color;
            Price = price;
        }

        public abstract string DisplayDetails();
    }

    class Refrigerator : Appliance
    {
        public int NumberOfDoors { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Refrigerator(string itemNumber, string brand, int quantity, int wattage, string color, double price, int numberOfDoors, int height, int width)
            : base(itemNumber, brand, quantity, wattage, color, price)
        {
            NumberOfDoors = numberOfDoors;
            Height = height;
            Width = width;
        }

        public override string DisplayDetails()
        {
            return $"Item Number: {ItemNumber}\nBrand: {Brand}\nQuantity: {Quantity}\nWattage: {Wattage}\nColor: {Color}\nPrice: ${Price:f2}\nNumber of Doors: {NumberOfDoors}\nHeight: {Height}\nWidth: {Width}";
        }
    }

    class Vacuum : Appliance
    {
        public string Grade { get; set; }
        public int BatteryVoltage { get; set; }

        public Vacuum(string itemNumber, string brand, int quantity, int wattage, string color, double price, string grade, int batteryVoltage)
            : base(itemNumber, brand, quantity, wattage, color, price)
        {
            Grade = grade;
            BatteryVoltage = batteryVoltage;
        }

        public override string DisplayDetails()
        {
            return $"Item Number: {ItemNumber}\nBrand: {Brand}\nQuantity: {Quantity}\nWattage: {Wattage}\nColor: {Color}\nPrice: ${Price:f2}\nGrade: {Grade}\nBattery Voltage: {(BatteryVoltage == 18 ? "Low" : "High")}";
        }
    }

    class Microwave : Appliance
    {
        public double Capacity { get; set; }
        public string RoomType { get; set; }

        public Microwave(string itemNumber, string brand, int quantity, int wattage, string color, double price, double capacity, string roomType)
            : base(itemNumber, brand, quantity, wattage, color, price)
        {
            Capacity = capacity;
            RoomType = roomType;
        }

        public override string DisplayDetails()
        {
            string roomTypeName = RoomType == "K" ? "Kitchen" : "Work site";
            return $"Item Number: {ItemNumber}\nBrand: {Brand}\nQuantity: {Quantity}\nWattage: {Wattage}\nColor: {Color}\nPrice: ${Price:f2}\nCapacity: {Capacity}\nRoom Type: {roomTypeName}";
        }
    }

    class Dishwasher : Appliance
    {
        public string Feature { get; set; }
        public string SoundRating { get; set; }

        public Dishwasher(string itemNumber, string brand, int quantity, int wattage, string color, double price, string feature, string soundRating)
            : base(itemNumber, brand, quantity, wattage, color, price)
        {
            Feature = feature;
            SoundRating = soundRating;
        }

        public override string DisplayDetails()
        {
            string soundRatingText = GetSoundRatingText(SoundRating);
            return $"Item Number: {ItemNumber}\nBrand: {Brand}\nQuantity: {Quantity}\nWattage: {Wattage}\nColor: {Color}\nPrice: ${Price:f2}\nFeature: {Feature}\nSound Rating: {soundRatingText}";
        }

        private string GetSoundRatingText(string soundRating)
        {
            switch (soundRating.ToLower())
            {
                case "qt":
                    return "Quietest";
                case "qr":
                    return "Quieter";
                case "qu":
                    return "Quiet";
                case "m":
                    return "Moderate";
                default:
                    return soundRating;
            }
        }
    }

    class Program
    {
        private static List<Appliance> appliances = new List<Appliance>();

        static void Main(string[] args)
        {
            LoadAppliances();

            while (true)
            {
                Console.WriteLine("\nWelcome to Modern Appliances!");
                Console.WriteLine("How may we assist you?");
                Console.WriteLine("1 - Check out appliance");
                Console.WriteLine("2 - Find appliances by brand");
                Console.WriteLine("3 - Display appliances by type");
                Console.WriteLine("4 - Produce random appliance list");
                Console.WriteLine("5 - Save & exit");
                Console.Write("Enter option: ");

                int option;
                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Invalid option. Please enter a number from 1 to 5.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        CheckOutAppliance();
                        break;
                    case 2:
                        FindAppliancesByBrand();
                        break;
                    case 3:
                        DisplayAppliancesByType();
                        break;
                    case 4:
                        ProduceRandomApplianceList();
                        break;
                    case 5:
                        SaveAppliances();
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void LoadAppliances()
        {
            try
            {
                foreach (var line in File.ReadLines("appliances.txt"))
                {
                    var parts = line.Split(';');
                    var itemNumber = parts[0];
                    var type = itemNumber[0];
                    switch (type)
                    {
                        case '1':
                            appliances.Add(new Refrigerator(itemNumber, parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]), int.Parse(parts[6]), int.Parse(parts[7]), int.Parse(parts[8])));
                            break;
                        case '2':
                            appliances.Add(new Vacuum(itemNumber, parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]), parts[6], int.Parse(parts[7])));
                            break;
                        case '3':
                            appliances.Add(new Microwave(itemNumber, parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]), double.Parse(parts[6]), parts[7]));
                            break;
                        case '4':
                        case '5':
                            appliances.Add(new Dishwasher(itemNumber, parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]), parts[6], parts[7]));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading appliances: {e.Message}");
            }
        }

        private static void SaveAppliances()
        {
            try
            {
                using (var writer = new StreamWriter("appliances.txt"))
                {
                    foreach (var appliance in appliances)
                    {
                        writer.WriteLine(appliance.DisplayDetails().Replace("\n", ";"));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving appliances: {e.Message}");
            }
        }

        private static void CheckOutAppliance()
        {
            Console.Write("Enter the item number of an appliance: ");
            var itemNumber = Console.ReadLine();
            var appliance = appliances.FirstOrDefault(a => a.ItemNumber == itemNumber);
            if (appliance != null)
            {
                if (appliance.Quantity > 0)
                {
                    appliance.Quantity--;
                    Console.WriteLine($"Appliance \"{itemNumber}\" has been checked out.");
                }
                else
                {
                    Console.WriteLine("The appliance is not available to be checked out.");
                }
            }
            else
            {
                Console.WriteLine("No appliances found with that item number.");
            }
        }

        private static void FindAppliancesByBrand()
        {
            Console.Write("Enter brand to search for: ");
            var brand = Console.ReadLine().ToLower();
            var found = false;
            foreach (var appliance in appliances)
            {
                if (appliance.Brand.ToLower() == brand)
                {
                    Console.WriteLine(appliance.DisplayDetails());
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("No appliances found for the given brand.");
            }
        }

        private static void DisplayAppliancesByType()
        {
            Console.WriteLine("Appliance Types");
            Console.WriteLine("1 - Refrigerators");
            Console.WriteLine("2 - Vacuums");
            Console.WriteLine("3 - Microwaves");
            Console.WriteLine("4 - Dishwashers");
            Console.Write("Enter type of appliance: ");
            int type;
            if (!int.TryParse(Console.ReadLine(), out type))
            {
                Console.WriteLine("Invalid appliance type.");
                return;
            }

            switch (type)
            {
                case 1:
                    Console.Write("Enter number of doors: 2 (double door), 3 (three doors), or 4 (four doors): ");
                    int doors;
                    if (!int.TryParse(Console.ReadLine(), out doors) || (doors != 2 && doors != 3 && doors != 4))
                    {
                        Console.WriteLine("Invalid number of doors.");
                        return;
                    }

                    var matchingRefrigerators = appliances.OfType<Refrigerator>().Where(r => r.NumberOfDoors == doors);
                    if (matchingRefrigerators.Any())
                    {
                        foreach (var refrigerator in matchingRefrigerators)
                        {
                            Console.WriteLine(refrigerator.DisplayDetails());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching refrigerators found.");
                    }
                    break;
                case 2:
                    Console.Write("Enter battery voltage value. 18 V (low) or 24 V (high): ");
                    int batteryVoltage;
                    if (!int.TryParse(Console.ReadLine(), out batteryVoltage) || (batteryVoltage != 18 && batteryVoltage != 24))
                    {
                        Console.WriteLine("Invalid battery voltage.");
                        return;
                    }

                    var matchingVacuums = appliances.OfType<Vacuum>().Where(v => v.BatteryVoltage == batteryVoltage);
                    if (matchingVacuums.Any())
                    {
                        foreach (var vacuum in matchingVacuums)
                        {
                            Console.WriteLine(vacuum.DisplayDetails());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching vacuums found.");
                    }
                    break;
                case 3:
                    Console.Write("Room where the microwave will be installed: K (kitchen) or W (work site): ");
                    var roomType = Console.ReadLine().ToUpper();
                    if (roomType != "K" && roomType != "W")
                    {
                        Console.WriteLine("Invalid room type.");
                        return;
                    }

                    var matchingMicrowaves = appliances.OfType<Microwave>().Where(m => m.RoomType == roomType);
                    if (matchingMicrowaves.Any())
                    {
                        foreach (var microwave in matchingMicrowaves)
                        {
                            Console.WriteLine(microwave.DisplayDetails());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching microwaves found.");
                    }
                    break;
                case 4:
                    Console.Write("Enter the sound rating of the dishwasher: Qt (Quietest), Qr (Quieter), Qu (Quiet), or M (Moderate): ");
                    var soundRating = Console.ReadLine().ToLower();
                    if (soundRating != "qt" && soundRating != "qr" && soundRating != "qu" && soundRating != "m")
                    {
                        Console.WriteLine("Invalid sound rating.");
                        return;
                    }

                    var matchingDishwashers = appliances.OfType<Dishwasher>().Where(d => d.SoundRating.ToLower() == soundRating);
                    if (matchingDishwashers.Any())
                    {
                        foreach (var dishwasher in matchingDishwashers)
                        {
                            Console.WriteLine(dishwasher.DisplayDetails());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching dishwashers found.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid appliance type.");
                    break;
            }
        }

        private static void ProduceRandomApplianceList()
        {
            Console.Write("Enter number of appliances: ");
            int count;
            if (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.WriteLine("Invalid number of appliances.");
                return;
            }

            var random = new Random();
            var selectedAppliances = new List<Appliance>();
            for (int i = 0; i < count; i++)
            {
                var randomIndex = random.Next(appliances.Count);
                selectedAppliances.Add(appliances[randomIndex]);
            }

            Console.WriteLine("Random appliances:");
            foreach (var appliance in selectedAppliances)
            {
                Console.WriteLine(appliance.DisplayDetails());
                Console.WriteLine();
            }
        }
    }
}
