using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP
{

    public class Population
    {

        // Member variables
        public List<Tour> p { get; private set; }
        public double maxFit { get; private set; }

        // ctor
        public Population(List<Tour> l)
        {
            this.p = l;
            this.maxFit = this.calcMaxFit();
        }

        // Functionality
        public static Population randomized(Tour t, int n)
        {
            List<Tour> tmp = new List<Tour>();

            for (int i = 0; i < n; ++i)
                tmp.Add( t.shuffle() );

            return new Population(tmp);
        }

        private double calcMaxFit()
        {
            return this.p.Max( t => t.fitness );
        }

        public Tour select()
        {
            while (true)
            {
                int i = Program.r.Next(0, Env.popSize);

                if (Program.r.NextDouble() < this.p[i].fitness / this.maxFit)
                    return new Tour(this.p[i].t);
            }
        }

        public Population genNewPop(int n)
        {
            List<Tour> p = new List<Tour>();

            for (int i = 0; i < n; ++i)
            {
                Tour t = this.select().crossover( this.select() );

                foreach (City c in t.t)
                    t = t.mutate();

                p.Add(t);
            }

            return new Population(p);
        }

        public Population elite(int n)
        {
            List<Tour> best = new List<Tour>();
            Population tmp = new Population(p);

            for (int i = 0; i < n; ++i)
            {
                best.Add( tmp.findBest() );
                tmp = new Population( tmp.p.Except(best).ToList() );
            }

            return new Population(best);
        }

        public Tour findBest()
        {
            foreach (Tour t in this.p)
            {
                if (t.fitness == this.maxFit)
                    return t;
            }

            // Should never happen, it's here to shut up the compiler
            return null;
        }

        public Population evolve()
        {
            Population best = this.elite(Env.elitism);
            Population np = this.genNewPop(Env.popSize - Env.elitism);
            return new Population( best.p.Concat(np.p).ToList() );
        }
    }
}

