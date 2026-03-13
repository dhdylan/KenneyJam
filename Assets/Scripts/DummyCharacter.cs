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

        _health.OnDamaged.AddListener(OnDamaged);

        _health.OnDeath.AddListener(OnDeath);
    }

    private void OnDamaged()
    {
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        Color originialColor = _mainSpriteRenderer.color;
        _mainSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        _mainSpriteRenderer.color = originialColor;
    }

    private void OnHurt(Hit hit)
    {
        Vector3 directionFromHit = (transform.position - hit.instigator.transform.position).normalized;
        _rigidbody2D.AddForce(directionFromHit * hit.knockbackAmount, ForceMode2D.Impulse);

        _health.AdjustHealth(-hit.damage);
        Debug.Log($"{gameObject.name} hurt for {hit.damage} damage by {hit.instigator.name}. Remaining health: {_health.GetCurrentHealth()}");
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
        _mainSpriteRenderer.color = Color.darkRed;

        yield return new WaitForSeconds(0.5f);

        float fadeTime = 3f;
        float timer = 0f;
        while(timer < fadeTime)
        {
            // fade out
            _mainSpriteRenderer.color = new Color(_mainSpriteRenderer.color.r, _mainSpriteRenderer.color.g, _mainSpriteRenderer.color.b, Mathf.Lerp(1f, 0f, timer / fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}