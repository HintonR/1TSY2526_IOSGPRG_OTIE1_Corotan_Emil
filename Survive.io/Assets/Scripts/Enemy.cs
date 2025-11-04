using UnityEngine;

public class Enemy : CharacterBase
{
    [SerializeField] private WeaponBase weapon;

    protected override void Awake()
    {
        base.Awake();
        if (weapon != null)
            EquipWeapon(weapon);
    }

    protected override bool HasAmmoFor(WeaponBase weapon) => true;

    protected override bool ConsumeAmmoFor(WeaponBase weapon)
    {
        return true;
    }

    protected override int ProvideAmmoForReload(WeaponBase weapon)
    {
        return weapon.MaxClipSize - weapon.CurrentClip;
    }
}