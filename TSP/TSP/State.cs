using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class State
    {
        public static State currentState;

        private double[,] matrix;
        private ArrayList route;
        private State includeChild;
        private State excludeChild;
        private State parent;
        private double bound;

        // Initial state
        public State(City[] cities)
        {
            matrix = new double[cities.Length, cities.Length];
            for (int i = 0; i < cities.Length; i++)
            {
                for (int j = 0; j < cities.Length; j++)
                {
                    matrix[i, j] = cities[i].costToGetTo(cities[j]);

                    // Set diagonals to infinity (negative)
                    if (i == j)
                    {
                        matrix[i, j] = -1;
                    }
                }
            }
            route = new ArrayList();

            currentState = this;
        }

        // Copy constructor for making children (Josh)
        public State(State state)
        {

        }

        // Expands this state, prunes, and finds next state to expand (Probably both of us)
        // Make sure to prevent creating a cycle prematurely
        public void Expand()
        {

        }

        // Finds initial BSSF (Brian)
        // Call after setting cost matrix
        public State findBSSF()
        {
            // TODO: fill this in
            return this;
        }

        // Reduces cost matrix and updates bound (Josh)
        public void reduceMatrix()
        {

        }

        // Returns bound (only after reducing matrix)
        public double Bound
        {
            get { return bound; }
        }

        // Returns current route
        public ArrayList Route
        {
            get { return route; }
        }

        // Gets priority (lower is better)
        public double Priority
        {
            get { return bound; }
        }

        // Recursively prune this branch (Brian)
        // Checks to see if it is better than BSSF
        public void prune(double bssf)
        {

        }
    }
}
