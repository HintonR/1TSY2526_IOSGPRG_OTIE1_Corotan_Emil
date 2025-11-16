using UnityEngine;

public class PickupCleanup : MonoBehaviour
{
    private PickupSpawner spawner;
    private Vector3 position;

    public void Init(PickupSpawner spawner, Vector3 pos)
    {
        this.spawner = spawner;
        position = pos;
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.RemovePosition(position);
    }
}
