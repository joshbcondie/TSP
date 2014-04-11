using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;
using System.Diagnostics;

namespace TSP
{
    class State : PriorityQueueNode
    {
        private static HeapPriorityQueue<State> queue = new HeapPriorityQueue<State>(2000000);
        // Top state, used to start recursive pruning
        private static State root;
        private static int cityCount;
        public static State BSSF;
        public static Stopwatch Watch = new Stopwatch();
        public static City[] cities;


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
        // We'll have to experiment to see what's most effective.
        public double Priority
        {
            get { return bound / (cityCount - pathsLeft + 1); }
        }

        private double[,] matrix;
        private ArrayList route;
        private State includeChild;
        private State excludeChild;
        private State parent;
        private double bound;
        private int depth;
        // To be used to avoid premature cycles
        private HashSet<int> usedVertices;
        private int pathsLeft;

        // Initial state, reduces matrix
        public State(City[] cities)
        {
            State.cities = cities;
            Watch.Start();
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
            usedVertices = new HashSet<int>();

            root = this;
            pathsLeft = cityCount;

            reduceMatrix();

            // Start expanding until agenda is empty, time is up, or BSSF cost is equal to original LB.
            Expand();
        }

        // Copy constructor for making children (Josh)
        // Reduces matrix based on whether edge is included
        // Adds state to queue
        public State(State parent, Boolean include, int from, int to)
        {
            matrix = (double[,])parent.matrix.Clone();
            route = new ArrayList(parent.route);
            usedVertices = parent.usedVertices;
            bound = parent.bound;
            this.parent = parent;
            depth = parent.depth + 1;

            if (include)
            {
                bound += matrix[from, to];
                pathsLeft = parent.pathsLeft - 1;

                for (int i = 0; i < cityCount; i++)
                {
                    matrix[from, i] = -1;
                    matrix[i, to] = -1;
                }
            }
            else
            {
                matrix[from, to] = -1;
            }

            reduceMatrix();

            // If there's more than one path left, delete paths to used vertices (Josh)
            if (pathsLeft > 1)
            {
                for (int i = 0; i < usedVertices.Count; i++)
                {
                    matrix[from, i] = -1;
                    // Shouldn't be necessary, but who knows?
                    matrix[i, to] = -1;
                }
            }

            // If not, set BSSF if necessary (Josh)
            else if (pathsLeft == 1)
            {
                for (int i = 0; i < cityCount; i++)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        if (matrix[i, j] >= 0)
                        {
                            includeChild = new State(this, true, i, j);
                            if (includeChild.bound < BSSF.bound)
                            {
                                BSSF = includeChild;
                                // Prune states starting with root
                                root.prune();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }

            // Add to queue
            if (pathsLeft > 0)
            {
                queue.Enqueue(this, Priority);
            }
        }

        // Constructor for initial BSSF
        public State(ArrayList iroute, double bound)
        {
            this.route = new ArrayList(iroute);
            this.bound = bound;
        }

        // Expands this state, prunes, and finds next state to expand (Probably both of us)
        // Make sure to prevent creating a cycle prematurely
        public void Expand()
        {
            // Check timeout
            if (Watch.Elapsed.Seconds >= 59)
            {
                Watch.Stop();
                return;
            }

            // Create two children and update usedVertices (Brian)
            if (pathsLeft > 0)
            {

            }

            // Find best state on queue and expand
            State best = queue.Dequeue();
            if (best == null || best.bound >= BSSF.bound || BSSF.bound == root.bound)
                return;
        }

        // Finds initial BSSF (Brian)
        // Call after setting cost matrix
        // Use vertices to the right (or left) of the diagonal
        // Cost matrix of initial BSSF, doesn't matter
        // Only the route and bound matter
        // So don't worry about the rest of the fields
        public State findBSSF()
        {
            ArrayList bssfCities = new ArrayList();
            double cost = 0;
            // TODO: fill this in
            for (int i = 0; i < cityCount; i++)
            {
                bssfCities.Add(cities[(i + 1) % cityCount]);
                cost += matrix[(i + 1) % cityCount, i];
            }
            return new State(bssfCities, cost);
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

        // Recursively prunes this branch (Brian)
        // Deletes descendents that don't make the cut
        public void prune()
        {

        }
    }
}
