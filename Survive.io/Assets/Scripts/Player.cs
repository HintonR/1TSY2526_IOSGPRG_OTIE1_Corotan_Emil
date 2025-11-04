using UnityEngine;

public class Player : CharacterBase
{

    GameManager _gM;

    [Header("Reserve Ammo")]
    [SerializeField] private int pistolAmmo;
    [SerializeField] private int shotgunAmmo;
    [SerializeField] private int rifleAmmo;

    [Header("Weapon Slots")]
    [SerializeField] private WeaponBase primaryWeapon;
    [SerializeField] private WeaponBase secondaryWeapon;

    public int PistolAmmo => pistolAmmo;
    public int ShotgunAmmo => shotgunAmmo;
    public int RifleAmmo => rifleAmmo;

    protected override void Awake()
    {
        base.Awake();
        _gM = GameManager.Instance;
        _gM.player = this;
        //if (primaryWeapon != null) EquipWeapon(primaryWeapon);
    }

    public void EquipPrimary() => EquipWeapon(primaryWeapon);
    public void EquipSecondary() => EquipWeapon(secondaryWeapon);

    protected override bool HasAmmoFor(WeaponBase weapon)
    {
        return weapon.AmmoType switch
        {
            AmmoType.Pistol => pistolAmmo > 0,
            AmmoType.Shotgun => shotgunAmmo > 0,
            AmmoType.Rifle => rifleAmmo > 0,
            _ => false
        };
    }

    protected override bool ConsumeAmmoFor(WeaponBase weapon)
    {
        switch (weapon.AmmoType)
        {
            case AmmoType.Pistol:
                if (pistolAmmo < weapon.AmmoPerShot) return false;
                pistolAmmo -= weapon.AmmoPerShot;
                break;

            case AmmoType.Shotgun:
                if (shotgunAmmo < weapon.AmmoPerShot) return false;
                shotgunAmmo -= weapon.AmmoPerShot;
                break;

            case AmmoType.Rifle:
                if (rifleAmmo < weapon.AmmoPerShot) return false;
                rifleAmmo -= weapon.AmmoPerShot;
                break;
        }

        OnAmmoChanged.Invoke();
        return true;
    }

    protected override int ProvideAmmoForReload(WeaponBase weapon)
    {
        int needed = weapon.MaxClipSize - weapon.CurrentClip;

        return weapon.AmmoType switch
        {
            AmmoType.Pistol => Mathf.Min(needed, pistolAmmo),
            AmmoType.Shotgun => Mathf.Min(needed, shotgunAmmo),
            AmmoType.Rifle => Mathf.Min(needed, rifleAmmo),
            _ => 0
        };
    }

    public void AddAmmo(AmmoType ammoType, int amount)
{
    switch (ammoType)
    {
        case AmmoType.Pistol:  pistolAmmo += amount;  break;
        case AmmoType.Shotgun: shotgunAmmo += amount; break;
        case AmmoType.Rifle:   rifleAmmo += amount;   break;
    }

    OnAmmoChanged?.Invoke();
}
}