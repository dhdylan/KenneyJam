using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MoleCharacter : Character
{
    [SerializeField] private Collider2D _mainCollider;
    [SerializeField] private Trigger _proximityTrigger;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private Hurtbox _hurtbox;
    [SerializeField] private Hitbox _hitbox;
    private Animator _animator;

    [SerializeField] private float _timeToRun = 5f;
    [SerializeField] private float _speedToRun = 1f;

    private float _timeStartedRunning = 0f;

    private Coroutine _runningCoroutine;

    protected override void Awake()
    {
        base.Awake();

        _hurtbox.OnHurt.AddListener(OnHurt);

        health.OnDamaged.AddListener(OnDamaged);

        health.OnDeath.AddListener(OnDeath);
    }

    private void Start()
    {
        _proximityTrigger.OnTriggerEnter.AddListener(OnObjectEnteredTrigger);
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
        _hurtbox.gameObject.SetActive(false);
        _hitbox.gameObject.SetActive(false);
        _mainCollider.excludeLayers = LayerMask.GetMask("Player"); // prevent further collisions with player and other enemies
        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        mainSpriteRenderer.color = Color.darkRed;

        yield return new WaitForSeconds(0.5f);

        float fadeTime = 3f;
        float timer = 0f;
        while (timer < fadeTime)
        {
            // fade out
            mainSpriteRenderer.color = new Color(mainSpriteRenderer.color.r, mainSpriteRenderer.color.g, mainSpriteRenderer.color.b, Mathf.Lerp(1f, 0f, timer / fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnObjectEnteredTrigger(GameObject go)
    {
        if(go.TryGetComponent<PlayerCharacter>(out PlayerCharacter player))
        {
            if (_runningCoroutine != null)
                return;

            _runningCoroutine = StartCoroutine(RunInDirection(go.transform.position.x < transform.position.x));
        }
    }

    private IEnumerator RunInDirection(bool left)
    {
        _timeStartedRunning = Time.time;

        while (Time.time - _timeStartedRunning < _timeToRun)
        {

            _rb2d.linearVelocityX = (left ? -1 : 1) * _speedToRun;

            yield return null;
        }

        _runningCoroutine = null;
    }
}