using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    GameManager _gM;

    [SerializeField] TextMeshProUGUI pistolAmmo;
    [SerializeField] TextMeshProUGUI rifleAmmo;
    [SerializeField] TextMeshProUGUI shotgunAmmo;

    [SerializeField] TextMeshProUGUI health;
    [SerializeField] Image hpBar;

    [SerializeField] RawImage primaryWeapon;
    [SerializeField] RawImage secondaryWeapon;


    private void Awake()
    {
        _gM = GameManager.Instance;
    }

    private void Update()
    {
        health.text = _gM.player.CurrentHealth.ToString();
        pistolAmmo.text = _gM.player.PistolAmmo.ToString();
        rifleAmmo.text = _gM.player.RifleAmmo.ToString();
        shotgunAmmo.text = _gM.player.ShotgunAmmo.ToString();
        hpBar.fillAmount = _gM.player.CurrentHealth / _gM.player.MaxHealth;
    }
}
