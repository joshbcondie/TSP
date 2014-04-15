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
        private static HeapPriorityQueue<State> queue;
        // Top state, used to start recursive pruning
        private static State root;
        private static int cityCount;
        public static State BSSF;
        public static Stopwatch Watch;
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
        private ArrayList visited;
        private int pathsLeft;

        // Initial state, reduces matrix
        public State(City[] cities)
        {
            queue = new HeapPriorityQueue<State>(10000);
            root = this;
            Watch = new Stopwatch();

            Watch.Start();
            State.cities = cities;
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
                        matrix[i, j] = double.PositiveInfinity;
                    }
                }
            }
            bound = 0;
            route = new ArrayList();
            visited = new ArrayList();

            pathsLeft = cityCount;
            setInitialBSSF();

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
            visited = new ArrayList(parent.visited);
            bound = parent.bound;
            this.parent = parent;
            depth = parent.depth + 1;

            if (include)
            {
                bound += matrix[from, to];
                pathsLeft = parent.pathsLeft - 1;
                visited.Add(from);
                visited.Add(to);

                for (int i = 0; i < cityCount; i++)
                {
                    matrix[from, i] = double.PositiveInfinity;
                    matrix[i, to] = double.PositiveInfinity;
                }

                // If there's more than one path left, delete paths to used vertices (Josh)
                if (pathsLeft > 1)
                {
                    for (int i = 0; i < visited.Count; i++)
                    {
                        matrix[to, (int)visited[i]] = double.PositiveInfinity;
                        // Shouldn't be necessary, but who knows?
                        matrix[(int)visited[i], from] = double.PositiveInfinity;
                    }
                }
            }
            else
            {
                matrix[from, to] = double.PositiveInfinity;
                pathsLeft = parent.pathsLeft;
            }

            reduceMatrix();

            // set BSSF if necessary (Josh)
            if (pathsLeft == 1)
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
                                // Calculate route from visited nodes
                                includeChild.calculateRoute();
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
            if (pathsLeft > 0 && bound < BSSF.bound)
            {
                queue.Enqueue(this, Priority);
            }
        }

        // Takes in visited cities and generates route
        private void calculateRoute()
        {
            int city = 0;
            for (int i = 0; i < cityCount; i++)
            {
                route.Add(cities[city]);
                int index = visited.IndexOf(city);
                if (index % 2 != 0)
                {
                    index = visited.IndexOf(city, index + 1);
                }
                city = (int)visited[index + 1];
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
            State current = this;

            while (current != null && BSSF.bound != root.bound)
            {
                // Check timeout
                if (Watch.Elapsed.Seconds >= 59)
                {
                    Watch.Stop();
                    return;
                }

                // Create two children and update usedVertices (Josh)
                if (current.pathsLeft > 0)
                {
                    for (int i = 0; i < cityCount; i++)
                    {
                        int j = 0;
                        for (j = 0; j < cityCount; j++)
                        {
                            if (current.matrix[i, j] == 0)
                            {
                                current.includeChild = new State(current, true, i, j);
                                current.excludeChild = new State(current, false, i, j);
                                break;
                            }
                        }
                        if (j < cityCount)
                        {
                            break;
                        }
                    }
                }

                // Find best state on queue and expand
                current = queue.Dequeue();
            }
        }

        // Finds initial BSSF (Brian)
        // Call after setting cost matrix
        // Use vertices to the right (or left) of the diagonal
        // Cost matrix of initial BSSF, doesn't matter
        // Only the route and bound matter
        // So don't worry about the rest of the fields
        public void setInitialBSSF()
        {
            ArrayList bssfCities = new ArrayList();
            double cost = 0;
            // TODO: fill this in
            for (int i = 0; i < cityCount; i++)
            {
                bssfCities.Add(cities[(i + 1) % cityCount]);
                cost += matrix[(i + 1) % cityCount, i];
            }
            BSSF = new State(bssfCities, cost);
        }

        // Reduces cost matrix and updates bound (Josh)
        public void reduceMatrix()
        {

            // Reduce rows
            // Checks for rows of infinity (negatives)
            for (int i = 0; i < cityCount; i++)
            {
                int infinityCount = 0;
                double min = double.PositiveInfinity;

                for (int j = 0; j < cityCount; j++)
                {
                    if (matrix[i, j] == double.PositiveInfinity )
                    {
                        infinityCount++;
                    }

                    else if (matrix[i,j] < min)
                    {
                        min = matrix[i, j];
                    }
                }

                if (infinityCount < cityCount)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        if (matrix[i, j] != double.PositiveInfinity && matrix[i, j] - min >= 0)
                        {
                            matrix[i, j] -= min;
                        }
                    }
                    bound += min;
                }
            }

            // Reduce columns
            for (int i = 0; i < cityCount; i++)
            {
                int infinityCount = 0;
                double min = double.PositiveInfinity;

                for (int j = 0; j < cityCount; j++)
                {
                    if (matrix[j, i] == double.PositiveInfinity)
                    {
                        infinityCount++;
                    }
                    else if (matrix[j, i] < min)
                    {
                        min = matrix[j, i];
                    }
                }

                if (infinityCount < cityCount)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        if (matrix[j, i] != double.PositiveInfinity && matrix[j, i] - min >= 0)
                        {
                            matrix[j, i] -= min;
                        }
                    }
                    bound += min;
                }
            }
        }

        // Recursively prunes this branch (Brian)
        // Deletes descendents that don't make the cut
        public void prune()
        {
            if (bound > BSSF.bound)
            {
                //queue.Remove(this);
            }
            if (includeChild != null)
                includeChild.prune();
            if (excludeChild != null)
                excludeChild.prune();
        }
    }
}
