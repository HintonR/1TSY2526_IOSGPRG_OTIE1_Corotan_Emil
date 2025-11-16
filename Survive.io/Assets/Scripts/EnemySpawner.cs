using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    GameManager _gM;
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float minDistanceBetweenEnemies;
    [SerializeField] private float minDistanceFromPlayer;

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private Transform player;

    private void Awake()
    {
        _gM = GameManager.Instance;
    }

    private void Start()
    {
        player = _gM.player.transform;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int spawnedCount = 0;
        const int attemptsPerEnemy = 15;

        while (spawnedCount < maxEnemies)
        {
            bool valid = false;
            Vector3 spawnPos = Vector3.zero;

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

                if (valid)
                    break;
            }

            if (!valid)
                break;

            GameObject instance = Instantiate(enemy, spawnPos, Quaternion.identity);

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