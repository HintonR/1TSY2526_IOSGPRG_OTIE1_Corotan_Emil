using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;   // Player transform
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset;     // Optional offset
    
    private void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Smooth follow movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Apply position (keep Z unchanged for 2D)
        smoothedPosition.z = transform.position.z;
        transform.position = smoothedPosition;
    }
}