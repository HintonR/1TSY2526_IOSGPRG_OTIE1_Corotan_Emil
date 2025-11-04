using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPickupData", menuName = "Game/Ammo Pickup")]
public class AmmoData : ScriptableObject
{
    public AmmoType ammoType;
    public int amount;
}