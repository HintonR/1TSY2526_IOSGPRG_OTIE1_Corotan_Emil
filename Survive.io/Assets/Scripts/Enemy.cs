using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("Weapon")]
    [SerializeField] private WeaponBase[] weapons;
    private WeaponBase weapon;

    protected override void Awake()
    {
        base.Awake();
        EquipWeapon(weapon);
    }

    private void Start()
    {
            int index = Random.Range(0, weapons.Length);
            weapon = Instantiate(weapons[index], transform);
            EquipWeapon(weapon);
    }

    public override bool FireWeapon(Vector2 aimDir)
    {
        if (weapon == null) return false;
        if (!weapon.CanFire) ReloadWeapon();
        return weapon.Fire(this, aimDir);
    }

    public override bool ReloadWeapon()
    {
        if (weapon == null) return false;

        int reserve = ProvideAmmoForReload(weapon);
        if (reserve <= 0) return false;

        int loaded = weapon.Reload(reserve);
        if (loaded > 0)
            ConsumeAmmoFor(weapon);

        return loaded > 0;
    }

    protected override bool HasAmmoFor(WeaponBase weapon)
    {
        return weapon.AmmoType switch
        {
            WeaponType.Pistol => 10,
            WeaponType.Shotgun => 5,
            WeaponType.Rifle => 15,
            _ => 0
        } > 0;
    }

    protected override bool ConsumeAmmoFor(WeaponBase weapon) => true;

    protected override int ProvideAmmoForReload(WeaponBase weapon)
    {
        return weapon.MaxClipSize - weapon.CurrentClip;
    }
}