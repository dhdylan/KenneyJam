using UnityEngine;

public class VelocityBasedTilt : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxAngle = 20f;        // Max tilt angle in degrees
    public float maxSpeed = 5f;         // Speed at which tilt reaches maxAngle

    [Header("Smoothing")]
    public float rotationSmoothTime = 0.1f;

    private Vector3 previousPosition;
    private float currentAngle = 0f;
    private float angleVelocity = 0f;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // Calculate velocity based on position delta
        Vector3 delta = transform.position - previousPosition;
        float velocityX = delta.x / Time.deltaTime;
        previousPosition = transform.position;

        // Normalize the velocity to [-1, 1] based on maxSpeed
        float normalized = Mathf.Clamp(velocityX / maxSpeed, -1f, 1f);

        // Map to target angle
        float targetAngle = normalized * -maxAngle;

        // Smooth the rotation
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref angleVelocity, rotationSmoothTime);

        // Apply the rotation on the Z axis
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
