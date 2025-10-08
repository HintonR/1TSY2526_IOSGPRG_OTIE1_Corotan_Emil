using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum Direction 
{
    Up,
    Down,
    Left,
    Right,
    Tap,
    None
}
public class SwipeManager : Singleton<SwipeManager>
{
    private Vector2 _fp, _lp;
    private float _dragThreshold, _tapThreshold;
    public Direction _direction;

    void Start()
    {
        _dragThreshold = Screen.height * 15 / 100;
        _tapThreshold = Screen.height * 5 / 100;
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _fp = touch.position;
                _lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _lp = touch.position;

                if (Mathf.Abs(_lp.x - _fp.x) > _dragThreshold || Mathf.Abs(_lp.y - _fp.y) > _dragThreshold)
                {
                    if (Mathf.Abs(_lp.x - _fp.x) > Mathf.Abs(_lp.y - _fp.y))
                    {   
                        if (_lp.x > _fp.x)
                        {   
                            Debug.Log("Right Swipe");
                            _direction = Direction.Right;
                        }
                        else
                        {   
                            Debug.Log("Left Swipe");
                            _direction = Direction.Left;
                        }
                    }
                    else 
                    {
                        if (_lp.y > _fp.y) 
                        {   
                            Debug.Log("Up Swipe");
                            _direction = Direction.Up;
                        }
                        else 
                        {   
                            Debug.Log("Down Swipe");
                            _direction = Direction.Down;
                        }
                    }
                }
                else
                {   
                    Debug.Log("Tap");
                    _direction = Direction.Tap;
                }
            }
            StartCoroutine(ResetDirection());
        }
    }
    private IEnumerator ResetDirection()
    {
        yield return new WaitForSeconds(0.5f);      
        _direction = Direction.None;
    }
}
