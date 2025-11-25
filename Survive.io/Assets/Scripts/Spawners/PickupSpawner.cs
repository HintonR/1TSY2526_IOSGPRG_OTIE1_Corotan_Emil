using UnityEngine;
using System.Collections.Generic;

public class PickupSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] ammoPickups;   
    [SerializeField] private GameObject[] weaponPickups;  
    [SerializeField] private int maxPickups;
    [SerializeField] private float minDistance;
    [Range(0f, 1f)] [SerializeField] private float ammoChance = 0.65f; 

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Obstacle Reference")]
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Start()
    {
        SpawnAllPickups();
    }

    private void SpawnAllPickups()
    {
        int spawnedCount = 0;
        int attemptsPerPickup = 15;

        while (spawnedCount < maxPickups)
        {
            Vector3 spawnPos = Vector3.zero;
            bool valid = false;

            for (int i = 0; i < attemptsPerPickup; i++)
            {
                spawnPos = GetRandomPositionInBounds();
                valid = true;

                foreach (var pos in spawnedPositions)
                {
                    if (Vector3.Distance(pos, spawnPos) < minDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                // check obstacle positions
                if (obstacleSpawner != null)
                {
                    foreach (var obsPos in obstacleSpawner.spawnedPositions)
                    {
                        if (Vector3.Distance(obsPos, spawnPos) < minDistance)
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                if (valid) break;
            }

            if (!valid) break;

            GameObject prefab;
            if (Random.value <= ammoChance && ammoPickups.Length > 0)
                prefab = ammoPickups[Random.Range(0, ammoPickups.Length)];
            else if (weaponPickups.Length > 0)
                prefab = weaponPickups[Random.Range(0, weaponPickups.Length)];
            else
                continue;

            Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedPositions.Add(spawnPos);
            spawnedCount++;
        }
    }

    private Vector3 GetRandomPositionInBounds()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0f);
    }
}