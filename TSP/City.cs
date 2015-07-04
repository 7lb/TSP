using System;

namespace TSP
{
    public class City
    {

        // Member variables
        public double x { get; private set; }
        public double y { get; private set; }

        // ctor
        public City(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Functionality
        public double distanceTo(City c)
        {
            return Math.Sqrt(Math.Pow((c.x - this.x), 2)
                            + Math.Pow((c.y - this.y), 2));
        }

        public static City random()
        {
            return new City( Program.r.NextDouble(), Program.r.NextDouble() );
        }
    }
}
