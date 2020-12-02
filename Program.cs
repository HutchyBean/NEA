using System;

namespace NEA
{
    class Program
    {
        static void Main(string[] args)
        {
            FlightManager manager = new FlightManager();
            manager.LoadAirports("Airports.txt");
            manager.DisplayMenu();
        }
    }
}
