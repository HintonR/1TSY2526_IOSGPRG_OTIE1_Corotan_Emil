using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Weapon")]
    [SerializeField] private WeaponBase equippedWeapon;
    [SerializeField] private Transform muzzle;
    
 
    public UnityEvent OnHealthChanged = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnWeaponChanged = new UnityEvent();
    public UnityEvent OnAmmoChanged = new UnityEvent();
    public UnityEvent<int, int> OnClipChanged = new UnityEvent<int, int>();

    public Transform Muzzle => muzzle;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public WeaponBase EquippedWeapon => equippedWeapon;
    public bool IsDead => currentHealth <= 0;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        OnHealthChanged.Invoke();

        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        currentHealth = 0;
        OnDeath.Invoke();
        gameObject.SetActive(false);
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        equippedWeapon = newWeapon;
        OnWeaponChanged.Invoke();
    }

    public virtual bool FireWeapon(Vector2 aimDirection)
    {
        if (equippedWeapon == null) return false;
        if (!HasAmmoFor(EquippedWeapon)) return false;
        if (!ConsumeAmmoFor(EquippedWeapon)) return false;
        
        if (!equippedWeapon.CanFire) return ReloadWeapon();
        
        return equippedWeapon.Fire(this, aimDirection);
    }

    public virtual bool ReloadWeapon()
    {
        if (equippedWeapon == null) return false;

        int reserveToUse = ProvideAmmoForReload(equippedWeapon);
        if (reserveToUse <= 0) return false;

        int used = equippedWeapon.Reload(reserveToUse);
        if (used > 0) OnAmmoChanged.Invoke();

        return used > 0;
    }

    protected abstract bool HasAmmoFor(WeaponBase weapon);
    protected abstract bool ConsumeAmmoFor(WeaponBase weapon);
    protected abstract int ProvideAmmoForReload(WeaponBase weapon);
}