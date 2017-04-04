/*! 
@file PQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief Priority Queue Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2014 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

A PQueue Class.

*/
using System;
using System.Collections.Generic;
using System.Collections;

namespace EpPathFinding.cs
{
      
    /// <summary>
    /// A template PriorityQueue class
    /// </summary>
    /// <typeparam name="T">Queue element type</typeparam>
    public class PQueue<T>:IQueue<T> where T : IComparable<T>
    {
        /// <summary>
        /// Reverse order comparer
        /// </summary>
        public sealed class ReverseOrderClass : IComparer<T>
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed. 
            int IComparer<T>.Compare(T x, T y)
            {
                return x.CompareTo(y) * -1;
            }
        }
        IComparer<T> queueComparer = new ReverseOrderClass();


        protected List<T> m_list = new List<T>();

        public PQueue()
        {
        }

        public PQueue(PQueue<T> b)
        {
            m_list = new List<T>(b.m_list);
        }

        /// <summary>
        /// Check if the queue is empty.
        /// </summary>
        /// <returns>true if the queue is empty, otherwise false.</returns>
        public virtual bool IsEmpty()
        {
            return m_list.Count == 0;
        }

        /// <summary>
        /// Check if the given item exists in the queue.
        /// </summary>
        /// <param name="queueItem">item to check</param>
        /// <returns>true if exists, otherwise false</returns>
        public virtual bool Contains(T queueItem)
        {
            return m_list.BinarySearch(queueItem, queueComparer) >= 0;
        }

        /// <summary>
        /// Return the number of element in the queue.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return m_list.Count;
            }
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public virtual T Peek()
        {
            return Front();
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public virtual T Front()
        {
            if (m_list == null)
                throw new ArgumentNullException();
            try
            {
                return m_list[m_list.Count - 1];
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }

        }

        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
        public virtual T Back()
        {
            if (m_list == null)
                throw new ArgumentNullException();
            try
            {
                return m_list[0];
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }

        }


        /// <summary>
        /// Insert the new item into the queue.
        /// </summary>
        /// <param name="queueItem">The inserting item.</param>
        public virtual void Enqueue(T queueItem)
        {
            int idx = m_list.BinarySearch(queueItem, queueComparer);
            if (idx <= 0)
            {
                m_list.Insert(~idx, queueItem);
            }
            else
            {
                m_list.Insert(idx, queueItem);
            }
        }


        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns>removed item</returns>
        public virtual T Dequeue()
        {
            T frontItem = m_list[m_list.Count - 1];
            m_list.RemoveAt(m_list.Count - 1);
            return frontItem;
        }

        /// <summary>
        /// Erase the given item from the queue.
        /// </summary>
        /// <param name="data">The data to erase.</param>
        /// <returns>true if successful, otherwise false</returns>
        public virtual bool Erase(T data)
        {
            int idx = m_list.BinarySearch(data, queueComparer);
            if (idx >= 0)
            {
                m_list.RemoveAt(idx);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Clear the queue
        /// </summary>
        public virtual void Clear()
        {
            m_list.Clear();
        }
              

        /// <summary>
        /// Return the actual queue structure
        /// </summary>
        /// <returns>the actual queue structure</returns>
        public virtual List<T> GetQueue()
        {
            return new List<T>(m_list);
        }


    }

}