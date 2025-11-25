using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("Weapon")]
    [SerializeField] private WeaponBase[] weapons;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        int index = Random.Range(0, weapons.Length);
        WeaponBase w = Instantiate(weapons[index], transform);
        EquipWeapon(w);
    }
    public override bool FireWeapon(Vector2 aimDir)
    {
        if (EquippedWeapon == null) return false;

        if (!EquippedWeapon.CanFire)
        {
            ReloadWeapon();
            return false;
        }

        return EquippedWeapon.Fire(this, aimDir);
    }



    public override bool HasAmmoFor(WeaponBase weapon)
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