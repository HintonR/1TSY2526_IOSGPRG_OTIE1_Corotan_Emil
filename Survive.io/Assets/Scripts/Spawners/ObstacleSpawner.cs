using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    GameManager _gM;

    [Header("Obstacle Settings")]
    [SerializeField] private GameObject[] highWeightObstacles;
    [SerializeField] private GameObject[] mediumWeightObstacles;
    [SerializeField] private GameObject[] lowWeightObstacles;

    [SerializeField] private int maxObstacles = 10;
    [SerializeField] private float minDistance = 1f;

    [Header("Player Distance")]
    [SerializeField] private float minDistanceFromPlayer = 3f;

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Spawn Chances")]
    [Range(0f,1f)] [SerializeField] private float highChance = 0.45f;
    [Range(0f,1f)] [SerializeField] private float mediumChance = 0.35f;

    public List<Vector3> spawnedPositions { get; private set; } = new List<Vector3>();

    private Transform player;

    private void Awake()
    {
        _gM = GameManager.Instance;
    }

    private void Start()
    {
        player = _gM.player.transform;
        SpawnAllObstacles();
    }

    private void SpawnAllObstacles()
    {
        int spawnedCount = 0;
        int attemptsPerObstacle = 15;

        while (spawnedCount < maxObstacles)
        {
            Vector3 spawnPos = Vector3.zero;
            bool valid = false;

            for (int i = 0; i < attemptsPerObstacle; i++)
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
                    if (Vector3.Distance(pos, spawnPos) < minDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid) break;
            }

            if (!valid) break;

            GameObject prefab = ChooseWeightedObstacle();
            if (prefab == null) continue;

            Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedPositions.Add(spawnPos);
            spawnedCount++;
        }
    }

    private GameObject ChooseWeightedObstacle()
    {
        float r = Random.value;

        if (r <= highChance && highWeightObstacles.Length > 0)
            return highWeightObstacles[Random.Range(0, highWeightObstacles.Length)];

        if (r <= highChance + mediumChance && mediumWeightObstacles.Length > 0)
            return mediumWeightObstacles[Random.Range(0, mediumWeightObstacles.Length)];

        if (lowWeightObstacles.Length > 0)
            return lowWeightObstacles[Random.Range(0, lowWeightObstacles.Length)];

        return null;
    }

    private Vector3 GetRandomPositionInBounds()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0f);
    }
}