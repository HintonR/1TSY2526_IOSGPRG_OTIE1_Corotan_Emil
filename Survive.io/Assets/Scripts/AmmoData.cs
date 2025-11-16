using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPickupData", menuName = "Game/Ammo Pickup")]
public class AmmoData : ScriptableObject
{
    public WeaponType ammoType;
    public int amount;
}