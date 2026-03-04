using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : Character
{
    [SerializeField] private Hitbox basicAttackHitbox;
    [SerializeField] private Hurtbox hurtbox;

    private Coroutine basicAttackCoroutine;

    protected override void Awake()
    {
        base.Awake();

        hurtbox.OnHurt.AddListener(OnPlayerHurt);
        health.OnDeath.AddListener(OnPlayerDied);
    }

    public void BasicAttack()
    {
        if(basicAttackCoroutine != null)
        {
            Debug.LogWarning("Basic attack already in progress. Restarting it.", this);
            StopCoroutine(basicAttackCoroutine);
        }

        basicAttackCoroutine = StartCoroutine(BasicAttackCoroutine());
    }

    private IEnumerator BasicAttackCoroutine()
    {
        // TODO: Play attack animation here
        // Enable hitbox for a short duration
        basicAttackHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        basicAttackHitbox.gameObject.SetActive(false);
    }

    public void ResetForRespawn()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
    }

    private void OnPlayerDied()
    {
        GameManager.instance.OnPlayerDied();
    }

    private void OnPlayerHurt(int damage)
    {
        health.AdjustHealth(-damage);
        Debug.Log($"Player hurt for {damage} damage. Remaining health: {health.GetCurrentHealth()}");
    }
}