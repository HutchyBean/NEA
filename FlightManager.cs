using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace  NEA
{
    class FlightManager {
        Dictionary<string, Airport> airports;
        Airport outAirport = default;
        string ukAirport = null;
        Aircraft craftType = default;
        int nFirstclass = -1;

        public void LoadAirports (string filename) {
            if(!File.Exists(filename)) {
                Console.WriteLine("Airports file not found");
                Environment.Exit(1);
            }

            Dictionary<string, Airport> airports = new Dictionary<string, Airport>();

            foreach(string line in File.ReadLines(filename)) {
                string[] splitted = line.Split(",");
                airports.Add(splitted[0].ToLower(), new Airport(splitted[0], splitted[1], Convert.ToInt32(splitted[2]),Convert.ToInt32(splitted[3])));
            }

            this.airports = airports;     
        }

        private void AirportDetails()
        {
            Console.Clear();
            Console.WriteLine("Select a UK Airport: ");
            var selection = Console.ReadLine().ToLower();
            if (selection != "lpl" && selection != "boh")
            {
                Console.Clear();
                Console.WriteLine("Not a valid UK Airport");
                Thread.Sleep(1500);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("Select a Valid Destination Code");
            var dSelection = Console.ReadLine().ToLower();
            if (!airports.ContainsKey(dSelection))
            {
                Console.Clear();
                Console.WriteLine("Not a Valid Destination");
                Thread.Sleep(1500);
                return;
            }

            ukAirport = selection;
            outAirport = airports[dSelection];

        }

        private void FlightDetails()
        {
            Console.Clear();
            Console.WriteLine("Select A Aircraft:");
            Console.WriteLine("1. Medium Narrow Body");
            Console.WriteLine("2. Large Narrow Body");
            Console.WriteLine("3. Medium Wide Body");
            Aircraft selection;
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    selection = new Aircraft("Medium Narrow Body", 8, 2650, 180, 8);
                    break;
                    
                case ConsoleKey.D2:
                    selection = new Aircraft("Wide Narrow Body", 7, 5600, 220, 10);
                    break;
                    
                case ConsoleKey.D3:
                    selection = new Aircraft("Medium Wide Body", 5, 4050, 406, 14);  
                    break;
                default:
                    Console.WriteLine("Not Valid Key");
                    return;
            }

            if (ukAirport == null)
            {
                Console.Clear();
                Console.WriteLine("There is no selected Airport to confirm Range");
                Thread.Sleep(1500);
                return;
            }

            if (selection.FlightRange < (ukAirport == "lpl" ? outAirport.disLpl : outAirport.disBoh))
            {
                Console.Clear();
                Console.WriteLine("That aircraft can not do a flight that long");
                Thread.Sleep(1500);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("How many first class seats? ");
            var seatsIn = Console.ReadLine();
            int seatsOut;
            if (!int.TryParse(seatsIn, out seatsOut))
            {
                Console.Clear();
                Console.WriteLine("Not a valid number");
                Thread.Sleep(1500);
                return;
            }

            if (seatsOut < 0 || (seatsOut > 0 && seatsOut < selection.MinFirstclass) ||
                seatsOut > selection.Capacity / 2)
            {
                Console.Clear();
                Console.WriteLine("Invalid number based on Aircraft");
                Thread.Sleep(1500);
                return;
            }

            craftType = selection;
            nFirstclass = seatsOut;

        }

        private void CalculateProfit()
        {
            var numStandard = craftType.Capacity - nFirstclass * 2;
            int stanPrice = 0;
            int fPrice = 0;
            Console.Clear();
            if (numStandard != 0)
            {
                
                Console.WriteLine("What is price of Standard Seat?");
                var stanPriceIn = Console.ReadLine();
                if (!int.TryParse(stanPriceIn, out stanPrice))
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number");
                    Thread.Sleep(1500);
                    return;
                }
            }

            if (nFirstclass != 0)
            {
                Console.Clear();
                Console.WriteLine("What is the price of a First Class Seat?");
                var fPriceIn = Console.ReadLine();
                if (!int.TryParse(fPriceIn, out fPrice))
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number");
                    Thread.Sleep(1500);
                    return;
                }
            }

            float costPerSeat = craftType.RunningCost * (ukAirport == "lpl" ? outAirport.disLpl : outAirport.disBoh) /
                              100f;

            float flightCost = costPerSeat * (numStandard + nFirstclass);

            float income = (nFirstclass * fPrice) + (numStandard * stanPrice);

            float profit = income - flightCost;
            if (profit > 0)
            {
                Console.WriteLine("The Final profit of the Flight is: " + profit.ToString("0.00"));
            }  else if (profit < 0)
            {
                Console.WriteLine("The Final loss of the Flight is: " + profit.ToString("0.00"));
            }
            else
            {
                Console.WriteLine("There is no loss or profit");
            }
            
            Console.ReadKey();
        }
        public void DisplayMenu() {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Selected UK Airport: " + (ukAirport ?? "Not Selected"));
                Console.WriteLine("Selected Destination: " + (outAirport.Equals(default(Airport)) ? "Not Selected" : outAirport.fullName));
                Console.WriteLine("Selected Aircraft: " + (craftType.Equals(default(Aircraft)) ? "Not Selected" : craftType.Name));
                Console.WriteLine("Selected  First Class Seats: " + (nFirstclass == -1 ? "Not Selected" : nFirstclass.ToString()));
            
                Console.WriteLine("\n1. Enter airport details");
                Console.WriteLine("2. Enter flight details");
                Console.WriteLine("3. Enter price plan and calculate profit");
                Console.WriteLine("4. Clear Data");
                Console.WriteLine("5. Quit");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        AirportDetails();
                        break;
                    
                    case ConsoleKey.D2:
                        FlightDetails();
                        break;
                    
                    case ConsoleKey.D3:
                        CalculateProfit();
                        break;
                    
                    case ConsoleKey.D4:
                        ukAirport = null;
                        outAirport = default;
                        nFirstclass = -1;
                        craftType = default;
                        break;
                 
                    case ConsoleKey.D5:
                        Environment.Exit(0);
                        break;
                }
            }
            
        }
    }    
}
