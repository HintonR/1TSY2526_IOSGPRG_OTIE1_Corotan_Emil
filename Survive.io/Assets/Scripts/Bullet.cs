using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            CharacterBase character = other.GetComponent<CharacterBase>();
            if (character != null)
                character.TakeDamage(damage);
                Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle"))
            Destroy(gameObject);
    }
}
