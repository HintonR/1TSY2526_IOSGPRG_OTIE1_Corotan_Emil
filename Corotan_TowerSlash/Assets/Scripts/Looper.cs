using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour
{
    GameManager _gM;
    public float _scrollSpeed = 350f;
    private float _height;
    private Vector3 _startPosition;

    void Start()
    {
        _gM = GameManager.Instance;
        _startPosition = transform.position;
        _height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        if (_gM._player.GetComponent<Player>().GetDashState()) _scrollSpeed = 20f;
        else _scrollSpeed = 1.5f;

        transform.position += Vector3.down * _scrollSpeed *  Time.deltaTime;
        if (transform.position.y <= _startPosition.y - _height)
            transform.position = new Vector3(transform.position.x, _startPosition.y, transform.position.z);
    }
}

