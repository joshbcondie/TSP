using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Priority_Queue;
using System.Diagnostics;
using OR_Bloggers.GeneralDatastructures;

namespace TSP
{
    class State : IComparable<State>
    {
        private static PriorityQueue<State> queue;
        // Top state, used to start recursive pruning
        private static State root;
        private static int cityCount;
        public static State BSSF;
        public static Stopwatch Watch;
        public static City[] cities;
        public static int maxQueueSize;


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
            //get { return bound / (cityCount - pathsLeft + 1); }
            get { return 0.0 * bound + 1000 * pathsLeft / cityCount; }
        }

        private double[,] matrix;
        private ArrayList route;
        private State includeChild;
        private State excludeChild;
        private double bound;
        private int depth;
        // To be used to avoid premature cycles
        private ArrayList visited;
        // For debugging purposes
        private ArrayList unvisited;
        private int pathsLeft;

        // Initial state, reduces matrix
        public State(City[] cities)
        {
            maxQueueSize = 0;
            queue = new PriorityQueue<State>();
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
                        matrix[i, j] = -1;
                    }
                }
            }
            bound = 0;
            route = new ArrayList();
            visited = new ArrayList();
            unvisited = new ArrayList();

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
            unvisited = new ArrayList(parent.unvisited);
            bound = parent.bound;
            depth = parent.depth + 1;

            if (include)
            {
                bound += matrix[from, to];
                for (int i = 0; i < cityCount; i++)
                {
                    // Check timeout
                    if (Watch.Elapsed.TotalSeconds >= 29)
                    {
                        return;
                    }

                    matrix[from, i] = -1;
                    matrix[i, to] = -1;
                }
                matrix[to, from] = -1;
                pathsLeft = parent.pathsLeft - 1;

                visited.Add(from);
                visited.Add(to);
            }
            else
            {
                matrix[from, to] = -1;
                pathsLeft = parent.pathsLeft;

                unvisited.Add(from);
                unvisited.Add(to);
            }

            reduceMatrix();

            // Set BSSF if necessary (Josh)
            if (pathsLeft == 0)
            {
                if (bound < BSSF.bound)
                {
                    calculateRoute();
                    //Console.WriteLine("Old: " + BSSF.bound);
                    //Console.WriteLine("New: " + bound);
                    BSSF = this;
                    root.prune();
                }
            }

            // Add to queue
            else if (bound < BSSF.bound)
            {
                queue.Enqueue(this);
                if (queue.Count > maxQueueSize)
                {
                    maxQueueSize = queue.Count;
                }
            }
        }

        private Boolean isValidToAdd(int from, int to)
        {
            if (pathsLeft == 1)
                return true;
            Debug.Assert(visited.IndexOf(to) % 2 == 0 || visited.IndexOf(to) == -1, "Shouldn't be going there");
            int next = to;
            int index = 0;
            while (true)
            {
                index = visited.IndexOf(next);
                if (index < 0)
                    return true;
                if (index % 2 == 1)
                {
                    index = visited.IndexOf(next, index + 1);
                    if (index < 0)
                        return true;
                }

                if (index + 1 == visited.Count)
                    return true;
                next = (int)visited[index + 1];
                if (next == from)
                    return false;
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
                if (Watch.Elapsed.TotalSeconds >= 29)
                {
                    Watch.Stop();
                    return;
                }

                if (current.bound >= BSSF.bound || current.pathsLeft == 0)
                {
                    try
                    {
                        current = queue.Dequeue();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    continue;
                }

                // Create two children and update usedVertices (Josh)
                for (int i = 0; i < cityCount; i++)
                {
                    int j = 0;
                    for (j = 0; j < cityCount; j++)
                    {
                        if (current.matrix[i, j] == 0 && current.isValidToAdd(i, j))
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

                if (current.includeChild == null && current.excludeChild == null)
                {
                    for (int i = 0; i < cityCount; i++)
                    {
                        int j = 0;
                        for (j = 0; j < cityCount; j++)
                        {
                            if (current.matrix[i, j] >= 0 && current.isValidToAdd(i, j))
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

                //Debug.Assert(current.includeChild != null && current.excludeChild != null, "Children not added");

                // Find best state on queue and expand
                try
                {
                    current = queue.Dequeue();
                }
                catch (Exception)
                {
                    return;
                }
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
            // Check timeout
            if (Watch.Elapsed.TotalSeconds >= 29)
            {
                return;
            }

            if (includeChild != null)
            {
                includeChild.prune();

                if (includeChild.bound >= BSSF.bound)
                {
                    //queue.Remove(includeChild);
                    //Console.WriteLine("After: " + queue.Count);
                    includeChild = null;
                }
            }

            if (excludeChild != null)
            {
                excludeChild.prune();

                if (excludeChild.bound >= BSSF.bound)
                {
                    //queue.Remove(excludeChild);
                    //Console.WriteLine("After: " + queue.Count);
                    excludeChild = null;
                }
            }


            // Older version
            /*if (bound > BSSF.bound)
            {
                try
                {
                    //queue.Remove(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("Problem removing from queue");
                }
            }
            if (includeChild != null)
                includeChild.prune();
            if (excludeChild != null)
                excludeChild.prune();*/
        }

        public int CompareTo(State state)
        {
            if (Priority.CompareTo(state.Priority) != 0)
            {
                return Priority.CompareTo(state.Priority);
            }
            return matrix.GetHashCode().CompareTo(state.matrix.GetHashCode());
        }
    }
}
