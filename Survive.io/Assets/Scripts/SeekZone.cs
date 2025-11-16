using UnityEngine;

public class SeekZone : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemyAI.SetSeekState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemyAI.SetSeekState(false);
    }
}