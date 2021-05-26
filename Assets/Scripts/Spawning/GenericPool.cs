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

        //currentPrefabCount = 0;
        prefabCount = listOfPrefabs.Count;
        if (prefabCount < 1)
        {
            Debug.LogWarning("No prefabs of objects are found.");
        }
    }

    private void Start()
    {
       AddObjects(prefabCount);
    }

    // get the next thing out of queue
    public T Get()
    {
        int objectCount = objects.Count;
        if (objectCount == 0)
        {
            AddObjects(1);
        }

        int randomId = Random.Range(0, objectCount);
        T currentObject = objects[randomId];
        objects.RemoveAt(randomId);
        return currentObject;
       
    }

    // things can return themselfes here
    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Add(objectToReturn);
    }

    // instantiate new object and enqueue it to pool
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
