using Animancer;
using UnityEngine;

public class ElectricPlatform : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SpriteRenderer _electricitySpriteRenderer;
    [SerializeField] private AnimancerComponent _animancerComponent;
    [SerializeField] private AnimationClip _electricityAnimationClip;
    [SerializeField] private float _secondsBetweenShocks = 3f;
    [SerializeField] private float _shockDuration = 1f;
    [Range(0f, 1f)]
    [Tooltip("This is a value 0-1 that represents how far into the on/off cycle this shock platform will start.\n" +
        "For example, if the time between shocks is 1s and the shock duration is 1s, setting this to 0.5\n" +
        "would result in the platforms electricity turning on immediately upon start.")]
    [SerializeField] private float _startPhase = 0f;
    [SerializeField] private Hitbox _hitbox;

    private float _lastShockEndTime = 0f;
    private float _shockPeriod = 0f;
    private bool _shockEnabled = false;

    private void Awake()
    {
        _shockPeriod = _shockDuration + _secondsBetweenShocks;
        _lastShockEndTime = Time.time - (_shockPeriod * _startPhase);
    }

    private void Start()
    {
        _hitbox.enabled = false;
        _electricitySpriteRenderer.enabled = false;
    }
    private void Update()
    {
        float period = _shockDuration + _secondsBetweenShocks;
                
        if (Time.time > period + _lastShockEndTime) // if time to turn shock off
        {
            _lastShockEndTime = Time.time;
            _shockEnabled = false;
            _hitbox.enabled = false;
            _animancerComponent.Stop();
            _electricitySpriteRenderer.enabled = false;
            _audioSource.Stop();
        }
        else if ((Time.time > _lastShockEndTime + _secondsBetweenShocks) && !_shockEnabled) // if time to turn on shock
        {
            _shockEnabled = true;
            _hitbox.enabled = true;
            _electricitySpriteRenderer.enabled = true;
            _audioSource.Play();
            _animancerComponent.Play(_electricityAnimationClip);
        }
    }

}
