using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * The Object Pool is designed to keep a contiguous list of active an inactive 
 * objects. This allows the program to do the most programatically expensive 
 * work of creating them at the start of the program, avoiding performance 
 * issues caused by instantiating objects during gameplay. It utilizes a queue 
 * structure to accomplish this.
 * **/
public sealed class ObjectPool<T> {

    private Queue<T> pool = new Queue<T>();
    private Func<T> maker;

    /// <summary>
    /// Creates an object pool with contents of type T.
    /// </summary>
    /// <param name="maker">A function that creates and returns a T 
    /// object.</param>
    /// <param name="size">The size of the pool.</param>
    /// <returns>The instantiated Object Pool.</returns>
    public ObjectPool(Func<T> maker, int size)
    {
        this.maker = maker;
        // Create <size> objects of type <T> using the maker() function
        for (int i = 0; i < size; i++)
        {
            pool.Enqueue(maker());
        }
    }

    /// <summary>
    /// Lends an object for use. Use "Pool.Return" to give it back.
    /// </summary>
    public T Rent()
    {
        // If the pool is empty, create an object. When it is returned, it will
        // be added to the pool
        if (pool.Count == 0)
        {
            Debug.Log("Pool overflow: " + typeof(T).ToString());
            return maker();
        }
        return pool.Dequeue();
    }

    /// <summary>
    /// Returns an object to the pool.
    /// </summary>
    public void Return(T toReturn)
    {
        pool.Enqueue(toReturn);
    }

    /// <summary>
    /// Returns a refernece to the contents of the pool.
    /// </summary>
    public Queue<T> Contents
    {
        get { return pool; }
    }

}
