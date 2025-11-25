using System.Collections;
using UnityEngine;

public class Rifle : WeaponBase
{
    [SerializeField] private int burstCount;
    [SerializeField] private float burstDelay;
    [SerializeField] private float fireCooldown = 0.5f;

    private bool canFire = true;

    public override bool Fire(CharacterBase owner, Vector2 aimDirection)
    {
        if (!CanFire || !canFire) return false;

        currentClip -= burstCount;
        owner.OnAmmoChanged.Invoke();

        _aM.PlayWeaponShotSFX(this);

        if (bullet != null && owner.Muzzle != null)
            owner.StartCoroutine(FireBurst(owner, aimDirection));

        owner.StartCoroutine(FireCooldownRoutine());
        return true;
    }

    private IEnumerator FireBurst(CharacterBase owner, Vector2 aimDirection)
    {
        for (int i = 0; i < burstCount; i++)
        {
            var b = Instantiate(bullet, owner.Muzzle.position, owner.Muzzle.rotation);
            b.GetComponent<Bullet>().Init(aimDirection);
            yield return new WaitForSeconds(burstDelay);
        }
    }

    private IEnumerator FireCooldownRoutine()
    {
        canFire = false;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}