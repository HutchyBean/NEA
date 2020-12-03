using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace  NEA
{
    class FlightManager {
        Dictionary<string, Airport> airports;
        Airport outAirport = default(Airport);
        string ukAirport = null;
        Aircraft craftType = default(Aircraft);
        int nFirstclass = -1;

        static void CPrint(string content, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(content + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LoadAirports (string filename) {
            if(!File.Exists(filename)) {
                CPrint("Airports file not found", ConsoleColor.Red);
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
                CPrint("Not a valid UK Airport",ConsoleColor.Red);
                Thread.Sleep(1500);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("Select a Valid Destination Code");
            var dSelection = Console.ReadLine().ToLower();
            if (!airports.ContainsKey(dSelection))
            {
                Console.Clear();
                CPrint("Not a Valid Destination",ConsoleColor.Red);
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
            switch (Console.ReadKey(true).Key)
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
                    Console.Clear();
                    CPrint("Not Valid Key", ConsoleColor.Red);
                    Thread.Sleep(1500);
                    return;
            }

            if (ukAirport == null)
            {
                Console.Clear();
                CPrint("There is no selected Airport to confirm Range",ConsoleColor.Red);
                Thread.Sleep(1500);
                return;
            }

            if (selection.FlightRange < (ukAirport == "lpl" ? outAirport.disLpl : outAirport.disBoh))
            {
                Console.Clear();
                CPrint("That aircraft can not do a flight that long", ConsoleColor.Red);
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
                CPrint("Not a valid number", ConsoleColor.Red);
                Thread.Sleep(1500);
                return;
            }

            if (seatsOut < 0 || (seatsOut > 0 && seatsOut < selection.MinFirstclass) ||
                seatsOut > selection.Capacity / 2)
            {
                Console.Clear();
                CPrint("Invalid number based on Aircraft", ConsoleColor.Red);
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
                    CPrint("Not a valid number",ConsoleColor.Red);
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
                    CPrint("Not a valid number", ConsoleColor.Red);
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
                Console.WriteLine("The Final loss of the Flight is: " + Math.Abs(profit).ToString("0.00"));
            }
            else
            {
                Console.WriteLine("There is no loss or profit");
            }
            
            Console.ReadKey(true);
        }
        public void DisplayMenu() {
            while (true)
            {
                Console.Clear();
                Console.Write("Selected UK Airport: ");
                CPrint(ukAirport ?? "Not Selected", ukAirport == null ? ConsoleColor.Red : ConsoleColor.Green);
                Console.Write("Selected Destination: ");
                CPrint(outAirport.Equals(default(Airport)) ? "Not Selected" : outAirport.fullName, outAirport.Equals(default(Airport)) ? ConsoleColor.Red : ConsoleColor.Green);
                Console.Write("Selected Aircraft: ");
                CPrint(craftType.Equals(default(Aircraft)) ? "Not Selected" : craftType.Name, craftType.Equals(default(Aircraft)) ? ConsoleColor.Red : ConsoleColor.Green);
                Console.Write("Selected  First Class Seats: ");
                CPrint(nFirstclass == -1 ? "Not Selected" : nFirstclass.ToString(), nFirstclass == -1 ? ConsoleColor.Red : ConsoleColor.Green);
                Console.WriteLine("\n1. Enter airport details");
                if (ukAirport == null) {
                    CPrint("2. Enter flight details", ConsoleColor.DarkGray);
                } else {
                    Console.WriteLine("2. Enter flight details");
                }
                
                if (nFirstclass == -1 ) {
                    CPrint("3. Enter price plan and calculate profit", ConsoleColor.DarkGray);
                } else {
                    Console.WriteLine("3. Enter price plan and calculate profit");
                }
                Console.WriteLine("4. Clear Data");
                Console.WriteLine("5. Quit");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        AirportDetails();
                        break;
                    
                    case ConsoleKey.D2:
                        if (ukAirport == null) {
                            Console.Clear();
                            CPrint("Airports are not selected: Needed For Range Calculations", ConsoleColor.Red);
                            Thread.Sleep(1500);
                            continue;
                        }
                        FlightDetails();
                        break;
                    
                    case ConsoleKey.D3:
                    if (nFirstclass == -1) {
                            Console.Clear();
                            CPrint("Flight Info is not set", ConsoleColor.Red);
                            Thread.Sleep(1500);
                            continue;
                        }
                        CalculateProfit();
                        break;
                    
                    case ConsoleKey.D4:
                        ukAirport = null;
                        outAirport = default(Airport);
                        nFirstclass = -1;
                        craftType = default(Aircraft);
                        break;
                 
                    case ConsoleKey.D5:
                        Environment.Exit(0);
                        break;
                }
            }
            
        }
    }    
}
