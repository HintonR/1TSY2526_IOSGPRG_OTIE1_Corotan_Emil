using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    GameManager _gM;

    [SerializeField]
    public TMPro.TextMeshProUGUI _score, _life, _gameOver;
    [SerializeField]
    public Button _dashButton, _retryButton, _playButton, _standard, _tank, _speed;
    [SerializeField]
    public Image _meter, _title;
    [SerializeField]
    public GameObject _dashGuage;

    void Start()
    {
        _gM = GameManager.Instance;
        
        _dashButton.onClick.AddListener(Dash);
        _retryButton.onClick.AddListener(Retry);
        _playButton.onClick.AddListener(Play);
        _standard.onClick.AddListener(() => ChooseType(Type.Standard));
        _tank.onClick.AddListener(() => ChooseType(Type.Tank));
        _speed.onClick.AddListener(() => ChooseType(Type.Speed));
    }

    void Update()
    {
        if (_gM._player.GetComponent<Player>().GetDash() < _gM._player.GetComponent<Player>().GetDashCap() && _gM._gState)
            _dashButton.gameObject.SetActive(false);
        else if (_gM._gState)
            _dashButton.gameObject.SetActive(true);
    }

    public void UpdateScore()
    {
        _score.text = "" + _gM._score;
    }
    public void UpdateLife()
    {
        _life.text = "Life: " + _gM._player.GetComponent<Player>().GetLife();
    }
    public void UpdateDash()
    {
        _meter.fillAmount = (float)_gM._player.GetComponent<Player>().GetDash() / (float)_gM._player.GetComponent<Player>().GetDashCap();
    }

    void Dash()
    {
        _gM._player.GetComponent<Player>().SetDashState(true);
        _gM._player.GetComponent<Player>().SetDash(0);
        UpdateDash();
        StartCoroutine(DashTimer());
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(10f);      
        _gM._player.GetComponent<Player>().SetDashState(false);
        
    }
    void Retry()
    {
        _retryButton.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);
       
        _playButton.gameObject.SetActive(true);
        _title.gameObject.SetActive(true);

        _life.gameObject.SetActive(false);
        _score.gameObject.SetActive(false);
        _dashGuage.gameObject.SetActive(false);
        _dashButton.gameObject.SetActive(false);

        _gM._player.GetComponent<Player>().SetLife(1);
    }
    void Play()
    {
        _playButton.gameObject.SetActive(false);
        _title.gameObject.SetActive(false);

        _standard.gameObject.SetActive(true);
        _tank.gameObject.SetActive(true);
        _speed.gameObject.SetActive(true);

        _gM._score = 0;
        UpdateScore();
    }

    void ChooseType(Type type)
    {
        _gM._player.GetComponent<Player>().SetType(type);
        _gM._player.GetComponent<Player>().UpdateType();
        StartGame();
    }

    void StartGame()
    {
        _standard.gameObject.SetActive(false);
        _tank.gameObject.SetActive(false);
        _speed.gameObject.SetActive(false);
       
        _gM._gState = true;
        UpdateDash();
        UpdateLife();
       
        _score.gameObject.SetActive(true);
        _life.gameObject.SetActive(true);
        _dashGuage.gameObject.SetActive(true);
    }
}
