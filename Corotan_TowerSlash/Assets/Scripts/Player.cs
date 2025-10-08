using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type
{
    Standard,
    Tank,
    Speed
} 
public class Player : MonoBehaviour
{
    GameManager _gM;
    SwipeManager _sW;
    UIManager _uiM;
    Animator _anims;
    int _life, _lifeCap, _dash, _dashCap;
    Type _type;
    bool _isDash;
    bool _isTapped = false;
    bool _isPointer;

    public void SetLife(int value) { _life = value; }
    public int GetLife() { return _life; }
    public void SetLifeCap(int value) { _lifeCap = value; }
    public void SetDash(int value) { _dash = value; }
    public int GetDash() { return _dash; }
    public int GetDashCap() { return _dashCap; }
    public bool GetDashState() { return _isDash; }
    public void SetDashState(bool value) { _isDash = value;}
    public void SetType (Type value) { _type = value; }

    void Start()
    {
        _gM = GameManager.Instance;
        _sW = SwipeManager.Instance;
        _uiM = UIManager.Instance;
        _anims = GetComponent<Animator>();
        
        _life = 1;

        _uiM.GetComponent<UIManager>().UpdateLife();
        _isDash = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
        
            if (touch.phase == TouchPhase.Began) _isPointer = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }

        if (_sW._direction == Direction.Tap && _gM._gState && _gM._enemySpawner.GetComponent<EnemySpawner>()._enemies.Count == 0 && !_isTapped)
        {
            if (!_isPointer)
            {
                _anims.SetBool("isTapping", true);
                StartCoroutine(ResetTapping());
                _isTapped = true;
                transform.Translate(new Vector3(0.1f, 0f, 0f));
            }
        }
    }

    private IEnumerator ResetTapping()
    {
        _gM._score += 10;
        _uiM.GetComponent<UIManager>().UpdateScore();
        yield return new WaitForSeconds(0.5f);        
        _anims.SetBool("isTapping", false);
        _isTapped = false;
    }

    public void UpdateType()
    {
        if (_type == Type.Standard)
        {
            _lifeCap = 4;
            _dashCap = 60;
        }
        else if (_type == Type.Tank)
        {
            _lifeCap = 6;
            _dashCap = 120;
        }
        else if (_type == Type.Speed)
        {
            _lifeCap = 2;
            _dashCap = 30;
        }
        _dash = 0;
        _life = _lifeCap;
    }

    public void GetHurt()
    {
        if (!_isDash)
        {
            _life--;
            _uiM.GetComponent<UIManager>().UpdateLife();
            _anims.SetBool("isHurt", true);
            StartCoroutine(ResetHurt());
        }
        else 
        { 
            _gM._score += 25;
            _uiM.GetComponent<UIManager>().UpdateScore();
        }
    }

    private IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(0.3f);
        _anims.SetBool("isHurt", false);
    }

    public void AddDash()
    {
        if (_dash < _dashCap) _dash += 5;
    }

    public void PowerUp()
    {
        int r = Random.Range(0, 100);
        //Debug.Log(r);
        if (r < 10)
        {
            if (_life < _lifeCap)
            { 
            _life++;
            _uiM.GetComponent<UIManager>().UpdateLife();
            //Debug.Log("Bonus!");
            }
        }
    }
}
