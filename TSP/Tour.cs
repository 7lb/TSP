using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP
{

    public class Tour
    {

        // Member variables
        public List<City> t { get; private set; }
        public double distance { get; private set; }
        public double fitness { get; private set; }

        // ctor
        public Tour(List<City> l)
        {
            this.t = l;
            this.distance = this.calcDist();
            this.fitness = this.calcFit();
        }

        // Functionality
        public static Tour random(int n)
        {
            List<City> t = new List<City>();

            for (int i = 0; i < n; ++i)
                t.Add( City.random() );

            return new Tour(t);
        }

        public Tour shuffle()
        {
            List<City> tmp = new List<City>(this.t);
            int n = tmp.Count;

            while (n > 1)
            {
                n--;
                int k = Program.r.Next(n + 1);
                City v = tmp[k];
                tmp[k] = tmp[n];
                tmp[n] = v;
            }

            return new Tour(tmp);
        }

        public Tour crossover(Tour m)
        {
            int i = Program.r.Next(0, m.t.Count);
            int j = Program.r.Next(i, m.t.Count);
            List<City> s = this.t.GetRange(i, j - i + 1);
            List<City> ms = m.t.Except(s).ToList();
            List<City> c = ms.Take(i)
                             .Concat(s)
                             .Concat( ms.Skip(i) )
                             .ToList();
            return new Tour(c);
        }

        public Tour mutate()
        {
            List<City> tmp = new List<City>(this.t);

            if (Program.r.NextDouble() < Env.mutRate)
            {
                int i = Program.r.Next(0, this.t.Count);
                int j = Program.r.Next(0, this.t.Count);
                City v = tmp[i];
                tmp[i] = tmp[j];
                tmp[j] = v;
            }

            return new Tour(tmp);
        }

        private double calcDist()
        {
            double total = 0;
            for (int i = 0; i < this.t.Count; ++i)
                total += this.t[i].distanceTo( this.t[ (i + 1) % this.t.Count ] );

            return total;

            // Execution time is doubled by using linq in this case
            //return this.t.Sum( c => c.distanceTo(this.t[ (this.t.IndexOf(c) + 1) % this.t.Count] ) );
        }

        private double calcFit()
        {
            return 1 / this.distance;
        }

    }
}

