using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ArrowColor
{
    Red,
    Green,
    Yellow
}

public class Enemy : MonoBehaviour
{
    GameManager _gM;
    SwipeManager _sW;
    UIManager _uiM;
    private ArrowColor _color;
    private Direction _direction;
    private GameObject _arrow;
    private GameObject _arrowInstance;
    public float arrowOffsetY = 1f;
    bool _isSpinning = false;
    private float _walkSpeed = 1.5f;

    public GameObject getArrow() { return _arrowInstance; }
    
    void Start()
    {
        _gM = GameManager.Instance;
        _sW = SwipeManager.Instance;
        _uiM = UIManager.Instance;

        _arrow = _gM._arrow;
        _color = (ArrowColor)Random.Range(0,3);
        //_color = ArrowColor.Yellow; //For Testing
        _direction = (Direction)Random.Range(0,4);
        if (_color == ArrowColor.Yellow) _isSpinning = true;
        
        //Debug.Log(_direction);
        SpawnArrowAbove();
        StartCoroutine(SpinArrow());
        _gM._enemySpawner.GetComponent<EnemySpawner>().AddEnemy(gameObject);
    }

    void Update()
    {
        if (_gM._player.GetComponent<Player>().GetDashState()) _walkSpeed = 20f;
        else _walkSpeed = 1.5f;

        transform.Translate(Vector3.right * _walkSpeed * Time.deltaTime); 

        if (_arrowInstance != null)
        {
            Vector3 arrowPosition = transform.position + new Vector3(-arrowOffsetY, 0 , 0);
            _arrowInstance.transform.position = arrowPosition;
        }

        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y)
        {
            Destroy(_arrowInstance);
            Destroy(gameObject);
            _gM._enemySpawner.GetComponent<EnemySpawner>().RemoveEnemy(gameObject);
        }
    }

    private IEnumerator SpinArrow()
    {
        while (true)
        {
            if (_isSpinning)
            {
            float currentZRotation = _arrowInstance.transform.rotation.eulerAngles.z;
            float newZRotation = currentZRotation + 90f;
            _arrowInstance.transform.rotation = Quaternion.Euler(0, 0, newZRotation);

            currentZRotation = Mathf.Round(currentZRotation) % 360;
            if (Mathf.Abs(currentZRotation - 90f) < 0.1f)       _direction = Direction.Right;
            else if (Mathf.Abs(currentZRotation - 180f) < 0.1f) _direction = Direction.Up;
            else if (Mathf.Abs(currentZRotation - 270f) < 0.1f) _direction = Direction.Left;
            else if (Mathf.Abs(currentZRotation - 0f) < 0.1f || 
                     Mathf.Abs(currentZRotation - 360f) < 0.1f) _direction = Direction.Down;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

     void OnTriggerEnter2D(Collider2D other)
    {   

        if (other.CompareTag("Attack"))
        {
            if (_color == ArrowColor.Green) _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._greenArrow;
            else if (_color == ArrowColor.Red) _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._redArrow;
            else if (_color == ArrowColor.Yellow) _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._yellowArrow;
        }
        else if (other.CompareTag("Sight"))
        {
            //Debug.Log("Enemy in Sight");
            _isSpinning = false;
            Debug.Log(_direction);
        }
        else if (other.CompareTag("Player"))
        {
            Attack();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            if (_color == ArrowColor.Green || _color == ArrowColor.Yellow) 
            {            
                if (_sW._direction == _direction) Killed();
                else if (_sW._direction != Direction.None) Attack();
                
            }
            else if (_color == ArrowColor.Red)
            {
                if (_direction == Direction.Up && _sW._direction == Direction.Down || 
                    _direction == Direction.Down && _sW._direction == Direction.Up ||
                    _direction == Direction.Left && _sW._direction == Direction.Right ||
                    _direction == Direction.Right && _sW._direction == Direction.Left)
                {
                    Killed();
                }
                else if (_sW._direction != Direction.None) Attack();
            }
        }
    }


    void SpawnArrowAbove()
    {
        if (_arrow != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(-arrowOffsetY, 0, 0);
            _arrowInstance = Instantiate(_arrow, spawnPosition, Quaternion.identity);
            if (_color == ArrowColor.Red)
            _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._redArrowN;
            else if (_color == ArrowColor.Green)
            _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._greenArrowN;
            else if (_color == ArrowColor.Yellow)
            _arrowInstance.GetComponent<SpriteRenderer>().sprite = _gM._yellowArrowN;
            SetArrowRotation();
        }
    }

    void SetArrowRotation()
    {
        if (_direction == Direction.Up) _arrowInstance.transform.rotation = Quaternion.Euler(0, 0, 270);
        else if (_direction == Direction.Down) _arrowInstance.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (_direction == Direction.Left) _arrowInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (_direction == Direction.Right) _arrowInstance.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void Killed()
    {
        Destroy(_arrowInstance);
        Destroy(gameObject);
        _gM._enemySpawner.GetComponent<EnemySpawner>().RemoveEnemy(gameObject);
        _gM._player.GetComponent<Player>().PowerUp();

        if (_gM._player.GetComponent<Player>().GetDash() < _gM._player.GetComponent<Player>().GetDashCap()) 
            _gM._player.GetComponent<Player>().AddDash();
        
        _gM._score += 25;
        _uiM.GetComponent<UIManager>().UpdateScore();
        _uiM.GetComponent<UIManager>().UpdateDash();
    }

    void Attack()
    {
            Destroy(_arrowInstance);
            Destroy(gameObject);
            _gM._enemySpawner.GetComponent<EnemySpawner>().RemoveEnemy(gameObject);
            _gM._player.GetComponent<Player>().GetHurt();
    }
}
