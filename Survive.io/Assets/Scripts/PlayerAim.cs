using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private Joystick aimJoystick; // assign Right joystick

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 15f;

    void Update()
    {
        Vector2 aimInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (aimInput.sqrMagnitude > 0.1f) // deadzone check
        {
            float angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }

    public Vector2 GetAimDirection()
{
    Vector2 aimInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

    if (aimInput.sqrMagnitude > 0.1f)
        return aimInput.normalized;

    return transform.right; 
}
}