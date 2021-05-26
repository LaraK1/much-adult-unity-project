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
        // triggers spawning at the current frequency if spawner should spawn
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

    /// <summary> Gets an object out of pool and resets it.</summary>
    private void Spawn()
    {
        // get object out of pool
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
    /// Can turn spawning things on and off. Reset timer.
    /// </summary>
    /// <param name="shouldSpawn">True - Spawning on; False - Spawning off</param>
    public void ToggleSpawning(bool shouldSpawn)
    {
        spawnTimer = 0;
        isSpawning = shouldSpawn;
    }
}
