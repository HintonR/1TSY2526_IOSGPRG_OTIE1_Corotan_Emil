using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{

    GameManager _gM;

    [SerializeField] TextMeshProUGUI pistolAmmo;
    [SerializeField] TextMeshProUGUI rifleAmmo;
    [SerializeField] TextMeshProUGUI shotgunAmmo;
    [SerializeField] TextMeshProUGUI clip;

    [SerializeField] TextMeshProUGUI health;
    [SerializeField] Image hpBar;

    [SerializeField] RawImage primaryWeapon;
    [SerializeField] RawImage secondaryWeapon;

    public UnityEvent<int, int> OnClipChanged = new UnityEvent<int, int>();
   
    private void Awake()
    {
        _gM = GameManager.Instance;
        _gM.player.OnWeaponChanged.AddListener(UpdateGunIcons);
        _gM.player.OnAmmoChanged.AddListener(UpdateAmmo);
        _gM.player.OnClipChanged.AddListener(UpdateClip);
        UpdateAmmo();
        UpdateClip(0,0);
    }

    private void Update()
    {
        health.text = _gM.player.CurrentHealth.ToString();
        hpBar.fillAmount = (float)_gM.player.CurrentHealth / (float)_gM.player.MaxHealth;
    }

    private void UpdateAmmo()
    {
        pistolAmmo.text = _gM.player.PistolAmmo.ToString();
        rifleAmmo.text = _gM.player.RifleAmmo.ToString();
        shotgunAmmo.text = _gM.player.ShotgunAmmo.ToString();
    }


private void UpdateGunIcons()
    {
        var player = _gM.player;
        if (player == null) return;

        WeaponBase active = player.ActiveWeapon;
        WeaponBase inactive = null;

        if (active == player.PrimaryWeapon)
            inactive = player.SecondaryWeapon;
        else
            inactive = player.PrimaryWeapon;

        if (active != null)
            primaryWeapon.texture = active.WeaponIcon;

        if (inactive != null)
            secondaryWeapon.texture = inactive.WeaponIcon;
    }

    private void UpdateClip(int currentClip, int maxClip)
    {
        clip.text = $"{currentClip}/{maxClip}";
    }
}
