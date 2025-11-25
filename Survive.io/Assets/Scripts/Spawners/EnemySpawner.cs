using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float minDistanceFromPlayer;
    [SerializeField] private float minDistanceBetweenEnemies;

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Obstacle Reference")]
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.player.transform;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int spawnedCount = 0;
        int attemptsPerEnemy = 15;

        while (spawnedCount < maxEnemies)
        {
            Vector3 spawnPos = Vector3.zero;
            bool valid = false;

            for (int i = 0; i < attemptsPerEnemy; i++)
            {
                spawnPos = GetRandomPositionInBounds();
                valid = true;

                if (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer)
                {
                    valid = false;
                    continue;
                }

                foreach (var pos in spawnedPositions)
                {
                    if (Vector3.Distance(pos, spawnPos) < minDistanceBetweenEnemies)
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
                        if (Vector3.Distance(obsPos, spawnPos) < minDistanceBetweenEnemies)
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                if (valid) break;
            }

            if (!valid) break;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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