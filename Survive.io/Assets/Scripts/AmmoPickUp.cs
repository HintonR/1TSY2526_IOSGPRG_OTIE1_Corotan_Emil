using UnityEngine;


public class AmmoPickup : MonoBehaviour
{
    AudioManager _aM;

    [SerializeField] private AmmoData data;

    private void Awake()
    {
        _aM = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        if (_aM.pickupSFX != null)
            _aM.PlaySFX(_aM.pickupSFX, _aM.pickupVolume);

        player.AddAmmo(data.ammoType, data.amount);
        Destroy(gameObject);
    }
}