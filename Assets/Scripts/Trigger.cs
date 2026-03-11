using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent<GameObject> OnTriggerEnter;
    public UnityEvent<GameObject> OnTriggerStay;
    public UnityEvent<GameObject> OnTriggerExit;

    [SerializeField] private BoxCollider2D _boxCollider;

    [SerializeField] private Color _gizmoColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter.Invoke(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit.Invoke(collision.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(_boxCollider.size.x, _boxCollider.size.y, 1.0f));
    }
}