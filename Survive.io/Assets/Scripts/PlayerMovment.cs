using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private Joystick moveJoystick; // assign Left joystick

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);

        if (input.magnitude > 1f)
            input = input.normalized;

        rb.linearVelocity  = input * moveSpeed;
    }
}