/*! 
@file IQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief Queue Interface
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

A Queue Interface.

*/
using System;
using System.Collections.Generic;
using System.Collections;

namespace EpPathFinding.cs
{
    /// <summary>
    /// A template Queue Interface class
    /// </summary>
    /// <typeparam name="T">Queue element type</typeparam>
    interface IQueue<T> where T : IComparable<T>
    {
        /// <summary>
        /// Check if the queue is empty.
        /// </summary>
        /// <returns>true if the queue is empty, otherwise false.</returns>
        bool IsEmpty();

        /// <summary>
        /// Check if the given item exists in the queue.
        /// </summary>
        /// <param name="queueItem">item to check</param>
        /// <returns>true if exists, otherwise false</returns>
        bool Contains(T queueItem);

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        T Peek();

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        T Front();
        
        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
        T Back();

        /// <summary>
        /// Insert the new item into the queue.
        /// </summary>
        /// <param name="queueItem">The inserting item.</param>
        void Enqueue(T queueItem);

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns>removed item</returns>
        T Dequeue();

        /// <summary>
        /// Erase the given item from the queue.
        /// </summary>
        /// <param name="data">The data to erase.</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Erase(T data);

        /// <summary>
        /// Clear the queue
        /// </summary>
        void Clear();

        /// <summary>
        /// Return the actual queue structure
        /// </summary>
        /// <returns>the actual queue structure</returns>
        List<T> GetQueue();

        /// <summary>
        /// Return the number of element in the queue.
        /// </summary>
        int Count
        {
            get;
        }
    }
}
