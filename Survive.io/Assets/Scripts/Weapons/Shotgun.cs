using UnityEngine;

public class Shotgun : WeaponBase
{
    [SerializeField] private int pelletCount;
    [SerializeField] private float spreadAngle;

    public override bool Fire(CharacterBase owner, Vector2 aimDirection)
    {
        if (!CanFire) return false;

        currentClip--;
        owner.OnAmmoChanged.Invoke();

        _aM.PlayWeaponShotSFX(this);

        if (bullet != null && owner.Muzzle != null)
        {
            float step = spreadAngle / (pelletCount - 1);
            float startAngle = -spreadAngle / 2f;

            for (int i = 0; i < pelletCount; i++)
            {
                float angle = startAngle + i * step;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * aimDirection;

                var b = Instantiate(bullet, owner.Muzzle.position, owner.Muzzle.rotation);
                b.GetComponent<Bullet>().Init(dir);
            }
        }

        return true;
    }
}