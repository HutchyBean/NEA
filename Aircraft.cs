namespace NEA
{
    public struct Aircraft
    {
        public string Name;
        public int RunningCost;
        public int FlightRange;
        public int Capacity;
        public int MinFirstclass;

        public Aircraft(string name, int runningCost, int flightRange, int capacity, int minFirstclass)
        {
            Name = name;
            RunningCost = runningCost;
            FlightRange = flightRange;
            Capacity = capacity;
            MinFirstclass = minFirstclass;
        }
        
    }
}