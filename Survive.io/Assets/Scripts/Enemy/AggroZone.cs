using UnityEngine;

public class AggroZone : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemyAI.SetAggroState(true);
    }

 //   private void OnTriggerExit2D(Collider2D other)
 //   {
 //       if (other.CompareTag("Player"))
 //           enemyAI.SetAggroState(false);
 //   }
}