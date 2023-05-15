using System;
using System.Collections.Generic;

namespace Covid19DistributionModel
{
    class Program
    {
        static void Main(string[] args)
        {
            List<City> cities = new List<City>();

            // Get the number of cities from the user
            Console.Write("Enter the number of cities: ");
            int numberOfCities = int.Parse(Console.ReadLine());

            // Get the details of each city from the user
            for (int i = 0; i < numberOfCities; i++)
            {
                Console.WriteLine($"City {i}:");
                Console.Write("Enter the name of the city: ");
                string cityName = Console.ReadLine();
                Console.Write("Enter the number of cities that this city contacts: ");
                int numberOfContacts = int.Parse(Console.ReadLine());

                List<int> contactCities = new List<int>();

                for (int j = 0; j < numberOfContacts; j++)
                {
                    Console.Write($"Enter the city number {j + 1} that contacts this city: ");
                    int contactCity = int.Parse(Console.ReadLine());

                    // Validate the contact city number
                    while (contactCity < 0 || contactCity >= numberOfCities || contactCities.Contains(contactCity) || contactCity == i)
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid city number.");
                        Console.Write($"Enter the city number {j + 1} that contacts this city: ");
                        contactCity = int.Parse(Console.ReadLine());
                    }

                    contactCities.Add(contactCity);
                }

                cities.Add(new City(i, cityName, contactCities));
            }

            // Display the initial city information
            Console.WriteLine("City number\tCity name\tCOVID-19 outbreak level");
            foreach (City city in cities)
            {
                Console.WriteLine($"{city.CityNumber}\t\t{city.CityName}\t\t{city.Covid19OutbreakLevel}");
            }

            // Receive event values from the user
            while (true)
            {
                Console.Write("Enter an event (Outbreak, Spread, Vaccinate, Lock down, or Exit): ");
                string eventType = Console.ReadLine();

                if (eventType == "Outbreak" || eventType == "Vaccinate" || eventType == "Lock down")
                {
                    Console.Write("Enter the city number where the event occurred: ");
                    int eventCityNumber = int.Parse(Console.ReadLine());

                    // Validate the event city number
                    while (eventCityNumber < 0 || eventCityNumber >= numberOfCities)
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid city number.");
                        Console.Write("Enter the city number where the event occurred: ");
                        eventCityNumber = int.Parse(Console.ReadLine());
                    }

                    // Process the event
                    ProcessEvent(eventType, cities[eventCityNumber]);

                    Console.WriteLine("City number\tCity name\tCOVID-19 outbreak level");
                    foreach (City city in cities)
                    {
                        Console.WriteLine($"{city.CityNumber}\t\t{city.CityName}\t\t{city.Covid19OutbreakLevel}");
                    }
                }
                else if (eventType == "Spread")
                {
                    SpreadCovid19(cities);

                    Console.WriteLine("City number\tCity name\tCOVID-19 outbreak level");
                    foreach (City city in cities)
                    {
                        Console.WriteLine($"{city.CityNumber}\t\t{city.CityName}\t\t{city.Covid19OutbreakLevel}");
                    }
                }
                else if (eventType == "Exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid event. Please enter a valid event.");
                }
            }
        }

        static void
ProcessEvent(string eventType, City city)
        {
            switch (eventType)
            {
                case "Outbreak":
                    city.Covid19OutbreakLevel += 2;
                    foreach (int contactCityNumber in city.ContactCities)
                    {
                        if (city.Covid19OutbreakLevel <= 3)
                        {
                            City contactCity = Program.GetCityByNumber(contactCityNumber);
                            contactCity.Covid19OutbreakLevel += 1;
                        }
                    }
                    break;
                case "Vaccinate":
                    city.Covid19OutbreakLevel = 0;
                    break;
                case "Lock down":
                    city.Covid19OutbreakLevel -= 1;
                    foreach (int contactCityNumber in city.ContactCities)
                    {
                        if (city.Covid19OutbreakLevel >= 0)
                        {
                            City contactCity = Program.GetCityByNumber(contactCityNumber);
                            contactCity.Covid19OutbreakLevel -= 1;
                        }
                    }
                    break;
            }
        }

        static void SpreadCovid19(List<City> cities)
        {
            foreach (City city in cities)
            {
                if (city.Covid19OutbreakLevel <= 2)
                {
                    city.Covid19OutbreakLevel += 1;
                    foreach (int contactCityNumber in city.ContactCities)
                    {
                        if (city.Covid19OutbreakLevel <= 3)
                        {
                            City contactCity = Program.GetCityByNumber(contactCityNumber);
                            contactCity.Covid19OutbreakLevel += 1;
                        }
                    }
                }
            }
        }

        static City GetCityByNumber(int cityNumber)
        {
            List<City> cities = new List<City>();
            // Assume that the cities list has been populated with cities
            return cities.Find(city => city.CityNumber == cityNumber);
        }
    }

    class City
    {
        public int CityNumber { get; set; }
        public string CityName { get; set; }
        public int Covid19OutbreakLevel { get; set; }
        public List<int> ContactCities { get; set; }

        public City(int cityNumber, string cityName, List<int> contactCities)
        {
            this.CityNumber = cityNumber;
            this.CityName = cityName;
            this.Covid19OutbreakLevel = 0;
            this.ContactCities = contactCities;
        }
    }
}