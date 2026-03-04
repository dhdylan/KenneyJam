using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class KillVolume : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCol;
    [SerializeField]
    private Vector2 size = Vector2.one;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.size = size;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.SetHealth(0);
        }
    }

    private void OnDrawGizmos()
    {
        boxCol.size = size;
        Gizmos.color = new Color(1.0f, 0.1f, 0.1f, 0.4f);


        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(size.x, size.y, 1.0f));
    }
}
