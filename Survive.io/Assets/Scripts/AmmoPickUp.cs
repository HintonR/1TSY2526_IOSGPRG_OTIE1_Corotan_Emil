using UnityEngine;


public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoData data;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        player.AddAmmo(data.ammoType, data.amount);
        Destroy(gameObject);
    }
}