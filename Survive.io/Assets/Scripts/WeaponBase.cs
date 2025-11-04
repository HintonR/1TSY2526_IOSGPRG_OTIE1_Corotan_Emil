using UnityEngine;
public enum AmmoType
{
    Pistol,
    Shotgun,
    Rifle
}
public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private string weaponName = "Weapon";
    [SerializeField] private AmmoType ammoType = AmmoType.Pistol;

    [Header("Firing")]
    [SerializeField] private int ammoPerShot;

    [Header("Clip Settings")]
    [SerializeField] private int maxClipSize;
    [SerializeField] private int currentClip;

    public string WeaponName => weaponName;
    public AmmoType AmmoType => ammoType;
    public int AmmoPerShot => ammoPerShot;

    public int MaxClipSize => maxClipSize;
    public int CurrentClip => currentClip;

    public bool CanFire => currentClip >= ammoPerShot;


    private void Start()
    {
        
    } 

    public virtual bool Fire(CharacterBase owner)
    {
        if (!CanFire) return false;

        currentClip -= ammoPerShot;
        owner.OnAmmoChanged.Invoke();
        return true;
    }

    public virtual int Reload(int amountFromReserve)
    {
        int needed = maxClipSize - currentClip;
        int toLoad = Mathf.Min(needed, amountFromReserve);

        currentClip += toLoad;
        return toLoad;
    }

    public void ForceReloadFull()
    {
        currentClip = maxClipSize;
    }
}