using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Hurtbox : MonoBehaviour
{
    public UnityEvent<Hit> OnHurt;

    private bool _invulnerable = false;
    private float _postHitInvulnerabilityTime = 0f;
    protected float _timeLastHit = 0f;

    protected void Update()
    {
        if (_invulnerable && Time.time > _timeLastHit + _postHitInvulnerabilityTime)
            SetInvulnerability(false);
    }

    public void Hurt(Hit hit)
    {
        if (hit == null)
        {
            Debug.LogError("Hurtbox received a null Hit.", this);
        }
        else
        {
            if (!_invulnerable)
            {
                SetInvulnerability(true);
                _timeLastHit = Time.time;
                OnHurt.Invoke(hit);
            }
        }
    }

    public float GetPostHitInvulnerabilityTime() { return _postHitInvulnerabilityTime;  }

    public void SetPostHitInvulnerabilityTime(float postHitInvulnerabilityTime)
    {
        _postHitInvulnerabilityTime = postHitInvulnerabilityTime;
    }

    public bool GetInvulnerability() { return _invulnerable; }

    private void SetInvulnerability(bool invulnerable)
    {
        _invulnerable = invulnerable;
    }
}