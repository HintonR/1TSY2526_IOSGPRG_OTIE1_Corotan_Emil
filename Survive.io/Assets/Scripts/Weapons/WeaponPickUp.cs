using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    AudioManager _aM;

    [SerializeField] private WeaponBase weaponPrefab;

    private void Awake()
    {
        _aM = AudioManager.Instance;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        if (TryGiveWeapon(player))
            Destroy(gameObject);
    }

    private bool TryGiveWeapon(Player player)
    {
        WeaponType type = weaponPrefab.AmmoType;

        switch (type)
        {
            case WeaponType.Pistol:
                GiveWeaponToPlayer(player, false);
                return true;

            case WeaponType.Shotgun:
            case WeaponType.Rifle:
                GiveWeaponToPlayer(player, true);
                return true;
        }

        return false;
    }

    private void GiveWeaponToPlayer(Player player, bool primary)
    {
        WeaponBase newWeapon = Instantiate(weaponPrefab);
        
        if (_aM.pickupSFX != null)
            _aM.PlaySFX(_aM.pickupSFX, _aM.pickupVolume);

        if (primary)
            player.EquipPrimary(newWeapon);
        else
            player.EquipSecondary(newWeapon);
    }
}