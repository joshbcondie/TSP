/* Copyright (C) 2013 OR-Bloggers.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * Furthermore we kindly ask that a reference to or-bloggers.com is made.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OR_Bloggers.GeneralDatastructures
{
    /// <summary>
    /// Represents a queue of objects that can be accessed by index, sorted, manipulated, searched and
    /// extracted in a prioritized order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the heap.</typeparam>
    public class PriorityQueue<T> : BinaryHeap<T>
    {
        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The queue is built
        /// using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new PriorityQueue&lt;T&gt;</param>
        public PriorityQueue(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The queue is built
        /// using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new PriorityQueue&lt;T&gt;.</param>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(IEnumerable<T> collection, Comparison<T> comparison) : base(collection, comparison) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The queue is built
        /// using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new PriorityQueue&lt;T&gt;.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer) : base(collection, comparer) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty with the specified initial capacity.
        /// The queue is built using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new queue can initially store.</param>
        public PriorityQueue(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty with the specified initial capacity. 
        /// The queue is built using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new queue can initially store.</param>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(int capacity, Comparison<T> comparison) : base(capacity, comparison) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty with the specified initial capacity. 
        /// The queue is built using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new queue can initially store.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(int capacity, IComparer<T> comparer) : base(capacity, comparer) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty and has the default initial capacity.
        /// The queue is built using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        public PriorityQueue() : base() { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty and has the default initial capacity.
        /// The queue is built using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(Comparison<T> comparison) : base(comparison) { }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; that is empty and has the default initial capacity.
        /// The queue is built using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public PriorityQueue(IComparer<T> comparer) : base(comparer) { }

        /// <summary>
        /// Adds and element to the bottom of the queue and then cascades the element upwards.
        /// </summary>
        /// <seealso cref="BinaryHeap&lt;T&gt;.Insert"/>
        /// <param name="element">The object to add to the PriorityQueue&lt;T&gt;.</param>
        public void Enqueue(T element)
        {
            Insert(element);
        }

        /// <summary>
        /// Removes and returns the first element in the PriorityQueue&lt;T&gt;.
        /// </summary>
        /// <seealso cref="BinaryHeap&lt;T&gt;.Extract"/>
        /// <exception cref="System.InvalidOperationException">Thrown if the heap is empty.</exception>
        /// <returns>The first element in the PriorityQueue&lt;T&gt;.</returns>
        public T Dequeue()
        {
            return Extract();
        }
    }

    /// <summary>
    /// Represents a binary heap of objects that can be accessed by index, sorted, manipulated, searched and
    /// extracted in a given order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the heap.</typeparam>
    public class BinaryHeap<T> : IEnumerable<T>
    {
        /// <summary>
        /// This class is the implementation described by D. Patrick Caldwell at 
        /// <see cref="!:http://dpatrickcaldwell.blogspot.dk/2009/04/converting-comparison-to-icomparer.html">Converting Comparison&lt;T&gt; to IComparer&lt;T&gt;</see>.
        /// This is used for converting System.Comparison&lt;T&gt; inputs to a 
        /// System.Collections.Generic.Comparer&lt;T&gt; which the binary heap uses.
        /// © 2008 — 2013, D. Patrick Caldwell, President, Autopilot Consulting, LLC
        /// </summary>
        private class ComparisonComparer : IComparer<T>
        {
            private readonly Comparison<T> _comparison;

            /// <summary>
            /// The constructor creating the System.Collections.Generic.Comparer&lt;T&gt; based on the given 
            /// System.Comparison&lt;T&gt;.
            /// </summary>
            /// <param name="comparison">The System.Comparison&lt;T&gt; to use for the comparer.</param>
            public ComparisonComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }

            /// <summary>
            /// The compare method. Returns an integer specified by the return value of the given comparison.
            /// </summary>
            /// <param name="x">The first value to compare.</param>
            /// <param name="y">The second value to compare.</param>
            /// <returns>The integer returned by the comparison based on the two given objects.</returns>
            public int Compare(T x, T y)
            {
                return _comparison(x, y);
            }
        }

        /// <summary>
        /// Gets the number of elements actually contained in the binary heap.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the capacity is tried to set to a 
        /// value which is smaller than the number of elements in the heap</exception>
        public int Capacity
        {
            get
            {
                return elements.Length;
            }
            set
            {
                if (value < Count)
                {
                    throw new InvalidOperationException("The capacity of the heap cannot be set to a value smaller than the size");
                }
                if (elements.Length != value)
                {
                    T[] currentElements = elements;
                    elements = new T[value];
                    for (int i = 0; i < Count; i++)
                    {
                        elements[i] = currentElements[i];
                    }
                }
            }
        }

        /// <summary>
        /// The minimum size of the heap.
        /// </summary>
        private const int START_HEAP_SIZE = 4;

        /// <summary>
        /// The array of elements in the internal data structure.
        /// </summary>
        private T[] elements;

        /// <summary>
        /// The System.Collections.Generic.Comparer&lt;T&gt; to use in the binary heap.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The heap is built
        /// using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new BinaryHeap&lt;T&gt;</param>
        public BinaryHeap(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The heap is built
        /// using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new BinaryHeap&lt;T&gt;.</param>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(IEnumerable<T> collection, Comparison<T> comparison) : this(collection, new ComparisonComparer(comparison)) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that contains elements copied from the specified 
        /// collection and has sufficient capacity to accomodate the number of elements copied. The heap is built
        /// using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new BinaryHeap&lt;T&gt;.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(IEnumerable<T> collection, IComparer<T> comparer)
        {
            this.comparer = comparer;
            Count = collection.Count();
            elements = new T[Count];
            int i = 0;
            foreach (var element in collection)
            {
                elements[i] = element;
                i++;
            }
            BuildHeap();
        }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty with the specified initial capacity.
        /// The heap is built using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new heap can initially store.</param>
        public BinaryHeap(int capacity) : this(capacity, Comparer<T>.Default) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty with the specified initial capacity. 
        /// The heap is built using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new heap can initially store.</param>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(int capacity, Comparison<T> comparison) : this(capacity, new ComparisonComparer(comparison)) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty with the specified initial capacity. 
        /// The heap is built using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="capacity">The number of elements that the new heap can initially store.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(int capacity, IComparer<T> comparer)
        {
            this.comparer = comparer;
            elements = new T[capacity];
            Count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty and has the default initial capacity.
        /// The heap is built using the default System.Collections.Generic.Comparer&lt;T&gt;.
        /// </summary>
        public BinaryHeap() : this(Comparer<T>.Default) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty and has the default initial capacity.
        /// The heap is built using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(Comparison<T> comparison) : this(new ComparisonComparer(comparison)) { }

        /// <summary>
        /// Initializes a new instance of the BinaryHeap&lt;T&gt; that is empty and has the default initial capacity.
        /// The heap is built using the specified System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use when comparing elements.</param>
        public BinaryHeap(IComparer<T> comparer)
        {
            this.comparer = comparer;
            elements = new T[START_HEAP_SIZE];
            Count = 0;
        }

        /// <summary>
        /// Build the heap from scratch.
        /// </summary>
        private void BuildHeap()
        {
            for (int i = (Count >> 1) - 1; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        /// <summary>
        /// Changes the currently used System.Collections.Generic.IComparer&lt;T&gt; to the specified one. Note 
        /// that a call to this method runs in linear time since the heap needs to be updated when changing the 
        /// System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; to use subsequently.</param>
        public void ChangeComparer(IComparer<T> comparer)
        {
            this.comparer = comparer;
            BuildHeap();
        }

        /// <summary>
        /// Changes the currently used System.Collections.Generic.IComparer&lt;T&gt; to a new one using the 
        /// specified System.Comparison&lt;T&gt; when comparing elements. Note that a call to this method runs in 
        /// linear time since the heap needs to be updated when changing the System.Collections.Generic.IComparer&lt;T&gt;.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use subsequently when comparing elements.</param>
        public void ChangeComparison(Comparison<T> comparison)
        {
            this.comparer = new ComparisonComparer(comparison);
            BuildHeap();
        }

        /// <summary>
        /// Checks for the element at the specified index of the heap whether the heap property holds. If not
        /// it is checked whether the element should be cascaded upwards or downwards in the heap.
        /// </summary>
        /// <param name="index">The index in the internal storage of the element to cascade.</param>
        public void Cascade(int index)
        {
            //We may need to cascade upwards
            int i = index, p;
            bool cascadedUp = false;
            T dummy;
            while (i > 0)
            {
                p = (i - 1) >> 1; //An alternative to this expression could be p = (i - 1) / 2;
                if (comparer.Compare(elements[i], elements[p]) >= 0) break;

                dummy = elements[p];
                elements[p] = elements[i];
                elements[i] = dummy;

                i = p;
                cascadedUp = true;
            }

            if (!cascadedUp)
            {
                //We may need to cascade downwards
                Heapify(index);
            }
        }

        /// <summary>
        /// Searches for the specified element in the heap and removes it if found. Returns true is the element was
        /// found and removed and false otherwise. The comparer or comparison given when initializing the heap will
        /// be used when comparing the element to elements in the internal storage.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        /// <returns>true if the element was succesfully removed; otherwise, false.</returns>
        public bool Remove(T element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (comparer.Compare(element, elements[i]) == 0)
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes an element from the internal storage at the specified index.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if the specified index is out of bounds
        /// of the heap.</exception>
        /// <param name="index">The index of the element wanted to remove.</param>
        /// <returns>the element which was removed from the set of elements.</returns>
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException("The index specified for deletion is out of bounds");
            }
            T returnElement = elements[index];
            Count--;
            elements[index] = elements[Count];
            if (Count < (Capacity >> 2)) Capacity >>= 1;
            //Cascade element
            Cascade(index);
            return returnElement;
        }

        /// <summary>
        /// Checks whether the heap property holds for the element at the specified index. If not then the element
        /// is cascaded downwards.
        /// </summary>
        /// <param name="index">The index of the element to check the heap property for.</param>
        private void Heapify(int index)
        {
            int l, r, pivot, i = index;
            T dummy;
            while (i < Count)
            {
                l = (i << 1) + 1; //An alternative expression is l = 2 * i + 1;
                r = l + 1; //The right child has the index of the left child plus one.
                if (l < Count && comparer.Compare(elements[l], elements[i]) < 0) pivot = l;
                else pivot = i;
                if (r < Count && comparer.Compare(elements[r], elements[pivot]) < 0) pivot = r;

                if (pivot == i) break; //The heap priority holds so the algorithm stops.

                dummy = elements[i];
                elements[i] = elements[pivot];
                elements[pivot] = dummy;

                i = pivot;
            }
        }

        /// <summary>
        /// Adds and element to the bottom of the heap and then cascades the element upwards.
        /// </summary>
        /// <param name="element">The object to add to the BinaryHeap&lt;T&gt;.</param>
        public void Insert(T element)
        {
            if (Count == Capacity)
            {
                Capacity <<= 1; //This could just have been expressed as Capacity *= 2;
            }
            elements[Count] = element;
            Count++;
            Cascade(Count - 1);
        }

        /// <summary>
        /// Checks whether the heap is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        /// <summary>
        /// Peeks at the first element in the heap.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the heap is empty.</exception>
        /// <returns>the first element in the heap.</returns>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The heap is empty when trying to peek");
            }
            return elements[0];
        }

        /// <summary>
        /// Removes and returns the first element in the BinaryHeap&lt;T&gt;.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the heap is empty.</exception>
        /// <returns>the first element in the BinaryHeap&lt;T&gt;.</returns>
        public T Extract()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The heap is empty when trying to extract");
            }
            T returnElement = elements[0];
            Count--;
            elements[0] = elements[Count];
            if (Count < Capacity / 4)
            {
                if (Capacity / 2 < START_HEAP_SIZE) Capacity = START_HEAP_SIZE;
                else Capacity /= 2;
            }
            //Cascade down
            Heapify(0);
            return returnElement;
        }

        /// <summary>
        /// Removes all elements from the BinaryHeap&lt;T&gt; and uses the default initial capacity.
        /// </summary>
        public void Clear()
        {
            elements = new T[START_HEAP_SIZE];
            Count = 0;
        }

        /// <summary>
        /// Copies the elements in the heap to an array.
        /// </summary>
        /// <returns>the elements in the heap as an array.</returns>
        public T[] ToArray()
        {
            T[] returnArray = new T[Count];
            for (int i = 0; i < Count; i++) returnArray[i] = elements[i];
            return returnArray;
        }

        /// <summary>
        /// Checks whether the heap contains the specified element. The comparer or comparison specified when
        /// initializing the heap will be used to compare the elements.
        /// </summary>
        /// <param name="element">The object to search for.</param>
        /// <returns>true if the element is found in the heap; otherwise, false.</returns>
        public bool Contains(T element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (comparer.Compare(elements[i], element) == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Copies the binary heap to an array at the specified index.
        /// </summary>
        /// <param name="array">The array that is the destination of the copied elements.</param>
        /// <param name="arrayIndex">The zero-based index of the given array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(elements, 0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Uses the heapsort algorithm to sort the elements in the heap. Mostly implemented for fun as the method
        /// Sort() is usually more efficient for large data sets.
        /// </summary>
        /// <seealso cref="BinaryHeap&lt;T&gt;.Sort()"/>
        public void HeapSort()
        {
            int realCount = Count;
            T dummy;
            for (int i = realCount - 1; i > 0; i--)
            {
                dummy = elements[i];
                elements[i] = elements[0];
                elements[0] = dummy;
                Count--;
                Heapify(0);
            }
            Count = realCount;
            //The elements in the heap needs to be reversed since the heapsort puts them in the reversed ordering
            Array.Reverse(elements, 0, Count);
        }

        /// <summary>
        /// Sorts the elements in the internal storage by using the System.Collections.Generic.Comparer&lt;T&gt; specified in the initialization when comparing elements.
        /// </summary>
        public void Sort()
        {
            Array.Sort<T>(elements, 0, Count);
        }

        /// <summary>
        /// Sorts the elements in the internal storage by using the System.Collections.Generic.Comparer&lt;T&gt;
        /// specified when comparing elements.
        /// </summary>
        /// <param name="comparer">The System.Collections.Generic.Comparer&lt;T&gt; to use when comparing elements.</param>
        public void Sort(IComparer<T> comparer)
        {
            Array.Sort<T>(elements, 0, Count, comparer);
        }

        /// <summary>
        /// Sorts the elements in the internal storage by using the System.Comparison&lt;T&gt;
        /// specified when comparing elements.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public void Sort(Comparison<T> comparison)
        {
            Array.Sort<T>(elements, 0, Count, new ComparisonComparer(comparison));
        }

        /// <summary>
        /// Gets or sets the element in the heap at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>the element at the given index.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException("The index specified was out of the range of the elements in the heap.");
                }
                return elements[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException("The index specified was out of the range of the elements in the heap.");
                }
                elements[index] = value;
                //Since the element is changed it might be needed to cascade the element.
                Cascade(index);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements in the BinaryHeap&lt;T&gt;.
        /// </summary>
        /// <returns>the enumerator that iterates through the elements in the BinaryHeap&lt;T&gt;.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements in the BinaryHeap&lt;T&gt;.
        /// </summary>
        /// <returns>the enumerator that iterates through the elements in the BinaryHeap&lt;T&gt;.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the BinaryHeapEnumerator&lt;T&gt; that iterates through the elements in the BinaryHeap&lt;T&gt;.
        /// </summary>
        /// <returns>the BinaryHeapEnumerator&lt;T&gt; that iterates through the elements in the BinaryHeap&lt;T&gt;.</returns>
        public BinaryHeapEnumerator<T> GetEnumerator()
        {
            return new BinaryHeapEnumerator<T>(elements, Count);
        }
    }

    /// <summary>
    /// The enumerator used for enumerating the elements in an instance of the BinaryHeap class
    /// </summary>
    /// <typeparam name="T">The type of the elements in the binary heap</typeparam>
    public class BinaryHeapEnumerator<T> : IEnumerator<T>
    {
        private T[] elements;
        private int heapSize;

        int position = -1;

        /// <summary>
        /// Creates a new instance of the enumerator based on a given input
        /// </summary>
        /// <param name="data">An array of the data to enumerate</param>
        /// <param name="heapSize">The size of the heap to enumerate over, i.e. it will only be allowed to enumerate over the elements in the heap</param>
        public BinaryHeapEnumerator(T[] data, int heapSize)
        {
            elements = data;
            this.heapSize = heapSize;
        }

        /// <summary>
        /// Increments the internal index of the current BinaryHeapEnumerator&lt;T&gt; object 
        /// to the next element of the enumerated heap.
        /// </summary>
        /// <returns>true if the index is successfully incremented and within the enumerated heap; otherwise, false.</returns>
        public bool MoveNext()
        {
            position++;
            return (position < heapSize);
        }

        /// <summary>
        /// Initializes the index to a position logically before the first element of the enumerated heap.
        /// </summary>
        public void Reset()
        {
            position = -1;
        }

        /// <summary>
        /// Gets the currently referenced element in the heap enumerated by this BinaryHeapEnumerator&lt;T&gt; object.
        /// </summary>
        public T Current
        {
            get { return elements[position]; }
        }

        void IDisposable.Dispose() { }

        /// <summary>
        /// Gets the currently referenced element in the heap enumerated by this BinaryHeapEnumerator&lt;T&gt; object.
        /// </summary>
        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
