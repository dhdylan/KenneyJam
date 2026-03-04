using System.Collections;
using UnityEngine;

public class DummyCharacter : Character
{
    [SerializeField] private Hurtbox hurtbox;

    protected override void Awake()
    {
        base.Awake();

        hurtbox.OnHurt.AddListener(OnHurt);

        health.OnDamaged.AddListener(OnDamaged);

        health.OnDeath.AddListener(OnDeath);
    }

    private void OnDamaged()
    {
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        Color originialColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originialColor;
    }

    private void OnHurt(int damage)
    {
        health.AdjustHealth(-damage);
        Debug.Log($"{gameObject.name} hurt for {damage} damage. Remaining health: {health.GetCurrentHealth()}");
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}