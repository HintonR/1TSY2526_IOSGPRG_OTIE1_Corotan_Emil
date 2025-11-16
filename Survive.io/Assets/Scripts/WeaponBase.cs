using UnityEngine;

public enum WeaponType
{
    Pistol,
    Shotgun,
    Rifle
}

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform muzzle;

    protected int currentClip;

    public WeaponType AmmoType => data.weaponType;
    public Texture WeaponIcon => data.icon;

    public int MaxClipSize => data.clipSize;
    public int CurrentClip => currentClip;
    public bool CanFire => currentClip > 0;

    private void Awake()
    {
        currentClip = MaxClipSize;
    }

    public virtual bool Fire(CharacterBase owner, Vector2 aimDirection)
    {
        if (!CanFire) return false;

        currentClip--;
        owner.OnClipChanged?.Invoke(currentClip, MaxClipSize);
        return true;
    }

    public virtual int Reload(int reserve)
    {
        int needed = MaxClipSize - currentClip;
        int toLoad = Mathf.Min(needed, reserve);

        currentClip += toLoad;
        return toLoad; // Player reduces reserve by this much
    }

    public void ForceReloadFull()
    {
        currentClip = MaxClipSize;
    }
}