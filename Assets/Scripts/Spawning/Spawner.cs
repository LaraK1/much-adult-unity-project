using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private bool isSpawning = false;
    private float frequency = 1;

    private float spawnTimer = 0;

    void Update()
    {
        if (isSpawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= frequency)
            {
                spawnTimer = 0;
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        // get the next object in queue
        var thing = ThingPool.Instance.Get();

        // reset the object to the start position
        thing.transform.rotation = transform.rotation;
        thing.transform.position = transform.position;
        thing.gameObject.SetActive(true);
    }

    /// <summary> Adjusts speed in relation to points. </summary>
    public void ChangeFrequency(int points)
    {
        frequency = (10 / points) + 2;
    }

    /// <summary>
    /// Can turn spawning things on and off. Reset frequency to 3;
    /// </summary>
    /// <param name="shouldSpawn">True - Spawning on; False - Spawning off</param>
    public void ToggleSpawning(bool shouldSpawn)
    {
        spawnTimer = 0;
        frequency = 3f;
        isSpawning = shouldSpawn;
    }
}
