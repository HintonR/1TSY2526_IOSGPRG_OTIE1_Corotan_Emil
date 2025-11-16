using UnityEngine;

public class Pistol : WeaponBase
{
    public override bool Fire(CharacterBase owner, Vector2 aimDirection)
    {
        if (!CanFire) return false;

        currentClip--;
        owner.OnAmmoChanged.Invoke();

        if (bullet != null && owner.Muzzle != null)
        {
            var b = Instantiate(bullet, owner.Muzzle.position, owner.Muzzle.rotation);
            b.GetComponent<Bullet>().Init(aimDirection);
        }

        return true;
    }
}
