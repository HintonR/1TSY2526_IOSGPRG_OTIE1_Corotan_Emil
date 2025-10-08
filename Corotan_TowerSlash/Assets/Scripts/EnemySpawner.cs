using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager _gM;
    
    public List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    GameObject _enemy;
    private void Start()
    {
        _gM = GameManager.Instance;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (_gM._gState) SpawnEnemy();
            yield return new WaitForSeconds(Random.Range(3.33f, 6.66f));
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(1f, 6f, 0);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, -90); 
        Instantiate(_enemy, spawnPosition, spawnRotation);
    }

        public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }
    public void ClearEnemies()
    {
            if (_enemies.Count != 0) foreach (GameObject enemy in _enemies) 
            {
                if (enemy) 
                {
                    GameObject arrow = enemy.GetComponent<Enemy>().getArrow();
                    Destroy(arrow);
                }
                Destroy(enemy);
            }
            _enemies.Clear();
    }
}

