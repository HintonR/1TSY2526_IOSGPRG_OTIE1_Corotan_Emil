using UnityEngine;
using UnityEngine.Events;

public class Player : CharacterBase
{
    private GameManager _gM;

    [Header("Reserve Ammo")]
    [SerializeField] private int pistolAmmo;
    [SerializeField] private int shotgunAmmo;
    [SerializeField] private int rifleAmmo;

    [Header("Weapon Slots")]
    [SerializeField] private WeaponBase primaryWeapon;
    [SerializeField] private WeaponBase secondaryWeapon;

    [Header("Aim")]
    [SerializeField] protected PlayerAim playerAim;


    private WeaponBase activeWeapon;

 
    public int PistolAmmo => pistolAmmo;
    public int ShotgunAmmo => shotgunAmmo;
    public int RifleAmmo => rifleAmmo;
    public WeaponBase ActiveWeapon => activeWeapon;
    public WeaponBase PrimaryWeapon => primaryWeapon;
    public WeaponBase SecondaryWeapon => secondaryWeapon;

    protected override void Awake()
    {
        base.Awake();
        _gM = GameManager.Instance;
        _gM.player = this;

        activeWeapon = primaryWeapon != null ? primaryWeapon : secondaryWeapon;
    }

    public void EquipPrimary() => EquipWeapon(primaryWeapon);
    public void EquipSecondary() => EquipWeapon(secondaryWeapon);

    public void EquipPrimary(WeaponBase w)
    {
        primaryWeapon = w;
        activeWeapon = primaryWeapon;
        EquipWeapon(activeWeapon);
        InvokeClipChanged();
    }

    public void EquipSecondary(WeaponBase w)
    {
        secondaryWeapon = w;
        activeWeapon = secondaryWeapon;
        EquipWeapon(activeWeapon);
        InvokeClipChanged();
    }

        public void SwitchWeapon()
    {
        if (activeWeapon == primaryWeapon && secondaryWeapon != null)
            activeWeapon = secondaryWeapon;
        else if (activeWeapon == secondaryWeapon && primaryWeapon != null)
            activeWeapon = primaryWeapon;

        EquipWeapon(activeWeapon);
        InvokeClipChanged();
    }

    private void InvokeClipChanged()
    {
        if (activeWeapon != null)
            OnClipChanged?.Invoke(activeWeapon.CurrentClip, activeWeapon.MaxClipSize);
    }

    protected override bool HasAmmoFor(WeaponBase weapon)
    {
        return weapon.AmmoType switch
        {
            WeaponType.Pistol => pistolAmmo >= 0,
            WeaponType.Shotgun => shotgunAmmo >= 0,
            WeaponType.Rifle  => rifleAmmo >= 0,
            _ => false
        };
    }

    protected override bool ConsumeAmmoFor(WeaponBase weapon) => true;
    

    protected override int ProvideAmmoForReload(WeaponBase weapon)
    {
        int needed = weapon.MaxClipSize - weapon.CurrentClip;
        int provided = 0;

        switch (weapon.AmmoType)
        {
            case WeaponType.Pistol:
                provided = Mathf.Min(needed, pistolAmmo);
                pistolAmmo -= provided;
                break;
            case WeaponType.Shotgun:
                provided = Mathf.Min(needed, shotgunAmmo);
                shotgunAmmo -= provided;
                break;
            case WeaponType.Rifle:
                provided = Mathf.Min(needed, rifleAmmo);
                rifleAmmo -= provided;
                break;
        }

        if (provided > 0)
            OnAmmoChanged?.Invoke();

        return provided;
    }

    public void AddAmmo(WeaponType ammoType, int amount)
    {
        switch (ammoType)
        {
            case WeaponType.Pistol:  pistolAmmo += amount;  break;
            case WeaponType.Shotgun: shotgunAmmo += amount; break;
            case WeaponType.Rifle:   rifleAmmo += amount;   break;
        }

        OnAmmoChanged?.Invoke();
    }


    public void FireActiveWeapon()
    {
        Vector2 aimDir = playerAim.GetAimDirection();
        bool fired = FireWeapon(aimDir);

        if (fired)
            InvokeClipChanged();
    }

    public void ReloadActiveWeapon()
    {
        bool reloaded = ReloadWeapon();
        if (reloaded)
            InvokeClipChanged();
    }

}