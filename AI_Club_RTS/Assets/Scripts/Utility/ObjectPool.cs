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
 * issues caused by creating objects during gameplay. It utilizes a linked list
 * structure to achieve this.
 * 
 * While it may appear at first that a stack would be more efficient (and 
 * simpler), recall that the main advantage this model provides is that 
 * disabled objects stay in memory, ready to jump into place with a few short
 * commands. This is why a pointer to the first available object in a pool is
 * preserved.
 * 
 * While it would be a considerably simpler implementation if this class was
 * generic, I do not currently know how to do that while preserving the pooled
 * objects' static constructors (e.g. City.MakeCity()).
 * **/
public sealed class ObjectPool : MonoBehaviour {

    private const int SMALL_POOL = 10;
    private const int MEDIUM_POOL = 50;

    private LinkedList<City> cityPool;
    private LinkedList<Infantry> infantryPool;
    //private LinkedList<Tank> tankPool;

    private LinkedListNode<City> firstAvailableCity;
    private LinkedListNode<Infantry> firstAvailableInfantry;
    //private LinkedListNode<Tank> firstAvailableTank;

    /// <summary>
    /// Initializes and returns an Infantry unit.
    /// </summary>
    /// <param name="team">The Infantry's team.</param>
    /// <param name="position">The Infantry's position.</param>
    /// <returns></returns>
    public Infantry RetrieveInfantry(Team team = null, Vector3 position = default(Vector3))
    {
        // Check to make sure the current object isn't in use -- if it is, 
        // we've exhausted the list
        if (firstAvailableInfantry == null)
            throw new Exception("No free objects in Infantry pool!");
        // Grab the free city
        Infantry curr = firstAvailableInfantry.Value;
        // Mark it as unavailable
        firstAvailableInfantry.Value = null;
        // Move one spot up in the list
        firstAvailableInfantry = firstAvailableInfantry.Next;
        // Initialize and return the unit
        curr.Team = team;
        curr.transform.position = position;
        curr.gameObject.SetActive(true);
        return curr;
    }

    /// <summary>
    /// Frees an Infantry to return to the pool.
    /// </summary>
    /// <param name="infantry"></param>
    public void ReturnInfantry(Infantry infantry)
    {
        infantry.gameObject.SetActive(false);
        // The spot behind the first available city is guaranteed to be null if
        // the pool is being used correctly
        firstAvailableInfantry.Previous.Value = infantry;
        // Move one spot back in the linked list with the new value
        firstAvailableInfantry = firstAvailableInfantry.Previous;
    }

    /// <summary>
    /// Initializes and returns a Tank unit.
    /// </summary>
    /// <param name="team">The Tank's team.</param>
    /// <param name="position">The Tank's position.</param>
    public Tank RetrieveTank(Team team = null, Vector3 position = default(Vector3))
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Frees a Tank to return to the pool.
    /// </summary>
    public void ReturnTank(Tank tank)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes and returns a City unit.
    /// </summary>
    /// <param name="team">The Infantry's team.</param>
    /// <param name="position">The Infantry's position.</param>
    /// <returns></returns>
    public City RetrieveCity(Team team = null, Vector3 position = default(Vector3))
    {
        // Check to make sure the current object isn't in use -- if it is, 
        // we've exhausted the list
        if (firstAvailableCity == null)
            throw new Exception("No free objects in City pool!");
        // Grab the free city
        City curr = firstAvailableCity.Value;
        // Mark it as unavailable and move forward in the list
        firstAvailableCity.Value = null;
        firstAvailableCity = firstAvailableCity.Next;
        // Initialize and return the unit
        curr.Team = team;
        curr.transform.position = position;
        curr.gameObject.SetActive(true);
        return curr;
    }

    /// <summary>
    /// Frees a City to return to the pool.
    /// </summary>
    public void ReturnCity(City toFree)
    {
        toFree.gameObject.SetActive(false);
        // The spot behind the first available city is guaranteed to be null if
        // the pool is being used correctly
        firstAvailableCity.Previous.Value = toFree;
        // Move one spot back in the linked list with the new value
        firstAvailableCity = firstAvailableCity.Previous;
    }

    /// <summary>
    /// Constructs the pools.
    /// </summary>
    private void Awake()
    {
        MakeCityPool(SMALL_POOL);
        MakeInfantryPool(MEDIUM_POOL);
    }

    /// <summary>
    /// Constructs the infantry pool.
    /// </summary>
    private void MakeInfantryPool(int size)
    {
        // Make the first infantry and mark it as the first available.
        infantryPool.AddLast(Infantry.MakeInfantry());
        firstAvailableInfantry = infantryPool.First;
        // Make the rest
        for (int i = 1; i < size; i++)
        {
            infantryPool.AddLast(Infantry.MakeInfantry());
        }
    }

    /// <summary>
    /// Constructs the city pool.
    /// </summary>
    private void MakeCityPool(int size)
    {
        // Make the first city and mark it as the first available
        cityPool.AddLast(City.MakeCity());
        firstAvailableCity = cityPool.First;
        // Make the rest
        for (int i = 1; i < size; i++)
        {
            cityPool.AddLast(City.MakeCity());
        }
    }

}
