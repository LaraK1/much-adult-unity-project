using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPool<T> : MonoBehaviour where T : Component
{
    // prefab / type management
    // for editing in the inspector 
    [SerializeField]
    private List<T> listOfPrefabs = new List<T>();

    private int currentPrefabCount;
    private int prefabCount;

    // pool
    public static GenericPool<T> Instance { get; private set; }
    private List<T> objects = new List<T>();

    private void Awake()
    {    
        // simple generic singleton
        Instance = this;

        // check if prefabs are assigned
        prefabCount = listOfPrefabs.Count;
        if (prefabCount < 1)
        {
            Debug.LogWarning("No prefabs of objects are found.");
        }
    }

    private void Start()
    {
       // add all prefabs to the pool
       AddObjects(prefabCount);
    }

    /// <summary>Get random object out of pool.</summary>
    public T Get()
    {
        int objectCount = objects.Count;

        // create new objects if pool is empty
        if (objectCount == 0)
        {
            AddObjects(1);
        }

        // random id
        int randomId = Random.Range(0, objectCount);

        // remove object out of pool and return it
        T currentObject = objects[randomId];
        objects.RemoveAt(randomId);
        return currentObject;
       
    }

    /// <summary>Objects can return themself to the pool.</summary>
    /// <param name="objectToReturn">Object that should be returned to the pool.</param>
    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Add(objectToReturn);
    }

    /// <summary>Instantiate new object and add it to pool.</summary>
    /// <param name="count">Number of objects to be added.</param>
    private void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObject = GameObject.Instantiate(listOfPrefabs[i]);
            newObject.gameObject.SetActive(false);
            objects.Add(newObject);

            currentPrefabCount++;
            if (currentPrefabCount > prefabCount)
            {
                currentPrefabCount = 0;
            }
        }
    }

}
