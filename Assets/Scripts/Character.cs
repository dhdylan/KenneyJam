using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public abstract class Character : MonoBehaviour
{
    // TODO:  Need to just get rid of these C# properties altogether and replace them with concrete getters and setters.
    protected Rigidbody2D _rigidbody2D;
    protected Health _health;

    [SerializeField] protected Hurtbox _hurtbox;
    [SerializeField] protected SpriteRenderer _mainSpriteRenderer;

    private Coroutine _flashRedCoroutine;
    private Color _originalSpriteColor;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        _originalSpriteColor = _mainSpriteRenderer.color;
        _health.OnDamaged.AddListener(CallFlashRed);
    }

    public Health GetHealth() { return _health; }

    public Rigidbody2D GetRigidbody2D() { return _rigidbody2D; }

    private void CallFlashRed()
    {
        if (_flashRedCoroutine != null)
        {
            StopCoroutine(_flashRedCoroutine);
        }

        _flashRedCoroutine = StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        _mainSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _mainSpriteRenderer.color = _originalSpriteColor;
    }
}