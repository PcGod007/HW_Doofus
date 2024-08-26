using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The target (Doofus) to follow
    public float smoothSpeed = 0.125f;  // Smoothness of camera movement
    public Vector3 offset;  // Offset from the target

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position
            Vector3 desiredPosition = target.position + offset;
            // Smoothly move the camera towards the target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Optional: Make sure the camera is always looking at the target
            transform.LookAt(target);
        }
    }
}
