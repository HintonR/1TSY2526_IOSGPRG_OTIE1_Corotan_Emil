using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    UIManager _uiM;
    [SerializeField]
    public Sprite _redArrow, _greenArrow, _yellowArrow, _redArrowN, _greenArrowN, _yellowArrowN;
    [SerializeField]
    public GameObject _arrow, _player, _enemySpawner;
    public int _score;
    public bool _gState = false;

    void Awake()
    {
        _score = 0;
    }
    
    void Start()
    {
        _uiM = UIManager.Instance;
    }

    void Update()
    {
        GameOver();
    }



    public void Quit()
    {
        Application.Quit();
    }

    void GameOver()
    {
        if (_player.GetComponent<Player>().GetLife() <= 0 && _gState) 
        {
            _gState = false;
            _enemySpawner.GetComponent<EnemySpawner>().ClearEnemies();
            _uiM._gameOver.gameObject.SetActive(true);
            _uiM._retryButton.gameObject.SetActive(true);
        }
    }
}
