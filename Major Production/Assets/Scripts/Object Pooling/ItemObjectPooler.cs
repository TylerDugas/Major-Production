﻿using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPooler : MonoBehaviour
{
    public static ItemObjectPooler s_instance;

    private int activeCount;
    public int PooledAmount;
    public List<GameObject> PooledObject;
    public List<GameObject> PooledObjects;

    private void Awake()
    {
        s_instance = this;
        PooledObjects = new List<GameObject>();
        foreach (var po in PooledObject)
        {
            for (var i = 0; i <= PooledAmount; i++)
            {
                var obj = Instantiate(po) as GameObject;
                obj.SetActive(false);
                obj.transform.SetParent(gameObject.transform);
                PooledObjects.Add(obj);
            }
        }
    }

    /// <summary>
    ///     Cant figure out how to use properly
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledGameObject()
    {
        foreach (var obj in PooledObjects)
            if (!obj.activeInHierarchy)
                return obj;
        return null;
    }

    /// <summary>
    ///     LifeCyle of create (1: activeCount greater than or equal to PooledAmount 2: activeCount Less than PooledAmount)
    ///     Create --1> AddToPool --1> Remove From Pool // Create --2> RemoveFromPool
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="objPos"></param>
    /// <param name="rot"></param>
    public GameObject Create(GameObject prefab, Vector3 objPos, Quaternion rot)
    {
        var newActiveCount = activeCount + 1;
        if (newActiveCount < PooledAmount)
        {
            RemoveFromPool(prefab);
            activeCount--;
        }
        else if (newActiveCount >= PooledAmount)
        {
            var newObj = Instantiate(prefab, objPos, rot);
            AddToPool(newObj);
            newObj.transform.SetParent(gameObject.transform);
            activeCount++;
            return newObj;
        }

        prefab.transform.position = objPos;
        prefab.transform.rotation = rot;
        return prefab;
    }

    public GameObject CreateSingle(GameObject prefab, Vector3 objPos, Quaternion rot)
    {
        RemoveFromPool(prefab);
        prefab.transform.position = objPos;
        prefab.transform.rotation = rot;
        activeCount--;
        return prefab;
    }

    /// <summary>
    ///     End User use case
    /// </summary>
    /// <param name="prefab"></param>
    public void Destroy(GameObject prefab)
    {
        AddToPool(prefab);
    }

    /// <summary>
    ///     Removes the object from pool by setting it active to true
    ///     then removing that object from the list
    /// </summary>
    /// <param name="prefab"></param>
    private void RemoveFromPool(GameObject prefab)
    {
        prefab.SetActive(true);
        PooledObjects.Remove(prefab);
    }

    /// <summary>
    ///     Adds item to pool by setting active to false
    ///     then adds that object to the list
    ///     Also checks if case number one and if so calls remove from pool
    /// </summary>
    /// <param name="prefab"></param>
    private void AddToPool(GameObject prefab)
    {
        prefab.SetActive(false);
        PooledObjects.Add(prefab);
    }
    
}