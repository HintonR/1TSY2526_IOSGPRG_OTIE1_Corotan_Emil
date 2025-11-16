using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameManager _gM;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float patrolChangeInterval;
    private Vector2 patrolDirection;
    private float patrolTimer = 0f;


    [Header("Aggro Settings")]
    [SerializeField] private float shootingInterval;
    [SerializeField] private float rotationSpeed;

    private Enemy enemy;
    private Player player;
    private float shootTimer = 0f;

    public enum State { Patrol, Seek, Aggro }
    public State currentState = State.Patrol;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        _gM = GameManager.Instance;
  
    }

    private void Start()
    {
              player = _gM.player;
    }

private void Update()
{
    switch (currentState)
    {
        case State.Patrol: Patrol(); break;
        case State.Seek:   Seek(); break;
        case State.Aggro:  Aggro(); break;
    }
}

private void Patrol()
{
    patrolTimer -= Time.deltaTime;

    if (patrolTimer <= 0f)
    {
        patrolDirection = Random.insideUnitCircle.normalized;
        patrolTimer = patrolChangeInterval;
    }

    transform.position += (Vector3)(patrolDirection * patrolSpeed * Time.deltaTime);

    if (patrolDirection.sqrMagnitude > 0.01f)
    {
        float targetAngle = Mathf.Atan2(patrolDirection.y, patrolDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.Euler(0f, 0f, targetAngle),
                                             Time.deltaTime * rotationSpeed);
    }
}

private void Seek()
{
    if (player == null) return;

    Vector2 moveDir = (player.transform.position - transform.position).normalized;
    transform.position += (Vector3)(moveDir * patrolSpeed * Time.deltaTime);

    if (moveDir.sqrMagnitude > 0.01f)
    {
        float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.Euler(0f, 0f, targetAngle),
                                             Time.deltaTime * rotationSpeed);
    }
}

private void Aggro()
{

    Vector2 aimDir = (player.transform.position - transform.position).normalized;

    float targetAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

    float step = rotationSpeed;
    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation,
                                                        Quaternion.Euler(0f, 0f, targetAngle),
                                                        step);

    shootTimer -= Time.deltaTime;
    if (shootTimer <= 0f)
    {
        enemy.FireWeapon(aimDir);
        shootTimer = shootingInterval;
    }
}

    public void SetAggroState(bool aggro)
    {
        currentState = aggro ? State.Aggro : State.Patrol;
    }

    
    public void SetSeekState(bool seek)
    {
        currentState = seek ? State.Seek : State.Patrol;
    }
}