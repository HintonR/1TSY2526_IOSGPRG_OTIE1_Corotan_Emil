using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponBase weaponPrefab;

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

        if (primary)
            player.EquipPrimary(newWeapon);
        else
            player.EquipSecondary(newWeapon);
    }
}