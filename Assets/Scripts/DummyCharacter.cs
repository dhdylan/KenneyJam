using System.Collections;
using UnityEngine;

public class DummyCharacter : Character
{
    [SerializeField] private Collider2D mainCollider;
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private Hitbox hitbox;

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
        Color originialColor = mainSpriteRenderer.color;
        mainSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        mainSpriteRenderer.color = originialColor;
    }

    private void OnHurt(Hit hit)
    {
        Vector3 directionFromHit = (transform.position - hit.instigator.transform.position).normalized;
        rigidbody2D.AddForce(directionFromHit * hit.knockbackAmount, ForceMode2D.Impulse);

        health.AdjustHealth(-hit.damage);
        Debug.Log($"{gameObject.name} hurt for {hit.damage} damage by {hit.instigator.name}. Remaining health: {health.GetCurrentHealth()}");
    }

    private void OnDeath()
    {
        hurtbox.gameObject.SetActive(false);
        hitbox.gameObject.SetActive(false);
        mainCollider.excludeLayers = LayerMask.GetMask("Player"); // prevent further collisions with player and other enemies
        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        mainSpriteRenderer.color = Color.darkRed;

        yield return new WaitForSeconds(0.5f);

        float fadeTime = 3f;
        float timer = 0f;
        while(timer < fadeTime)
        {
            // fade out
            mainSpriteRenderer.color = new Color(mainSpriteRenderer.color.r, mainSpriteRenderer.color.g, mainSpriteRenderer.color.b, Mathf.Lerp(1f, 0f, timer / fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}