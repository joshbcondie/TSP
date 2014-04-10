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
        private int depth;
        private int cityCount;

        // Initial state
        public State(City[] cities)
        {
            cityCount = cities.Length;
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
            bound = 0;
            route = new ArrayList();

            currentState = this;

            reduceMatrix();
        }

        // Copy constructor for making children (Josh)
        public State(State state)
        {
            matrix = (double[,])state.matrix.Clone();
            route = new ArrayList(state.route);
            bound = state.bound;
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

            // Reduce rows
            // Checks for rows of infinity (negatives)
            for (int i = 0; i < cityCount; i++)
            {
                double min = -1;

                for (int j = 0; j < cityCount; j++)
                {
                    if (min < 0 || (matrix[i, j] < min && matrix[i, j] >= 0))
                    {
                        min = matrix[i, j];
                    }
                }

                if (min > 0)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        matrix[i, j] -= min;
                    }
                    bound += min;
                }
            }

            // Reduce columns
            for (int i = 0; i < cityCount; i++)
            {
                double min = -1;

                for (int j = 0; j < cityCount; j++)
                {
                    if (min < 0 || (matrix[j, i] < min && matrix[j, i] >= 0))
                    {
                        min = matrix[j, i];
                    }
                }

                if (min > 0)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        matrix[j, i] -= min;
                    }
                    bound += min;
                }
            }
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
