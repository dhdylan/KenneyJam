using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    public float smoothTime = 0.2f;

    [Header("Axes")]
    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 desiredPosition = transform.position;

        if (followX) desiredPosition.x = targetPosition.x;
        if (followY) desiredPosition.y = targetPosition.y;
        if (followZ) desiredPosition.z = targetPosition.z;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
    }
}
