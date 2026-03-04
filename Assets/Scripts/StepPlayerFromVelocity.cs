using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StepPlayerFromVelocity : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D targetRigidbody;
    [SerializeField]
    private MovementController2D cc2d;

    [Header("Velocity Settings")]
    public float minVelocity = 0.1f;  // Start stepping at this speed
    public float maxVelocity = 5f;    // Max speed to map frequency

    [Header("Step Timing (in Hz)")]
    public float minStepRate = 1f;    // Steps per second at minVelocity
    public float maxStepRate = 5f;    // Steps per second at maxVelocity
    [Header("Step volume settings")]
    public float minVolume = .2f;
    public float maxVolume = 1.0f;

    private float stepCooldown;
    private float lastStepTime;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (cc2d.IsGrounded())
        {
            float speed = Mathf.Abs(targetRigidbody.linearVelocity.x);

            if (speed < minVelocity)
                return;

            // Normalize speed between minVelocity and maxVelocity
            float t = Mathf.InverseLerp(minVelocity, maxVelocity, speed);
            float stepRate = Mathf.Lerp(minStepRate, maxStepRate, t); // steps per second
            audioSource.volume = Mathf.SmoothStep(minVolume, maxVolume, t);
            stepCooldown = 1f / stepRate;

            if (Time.time >= lastStepTime + stepCooldown)
            {
                audioSource.Play();
                lastStepTime = Time.time;
            }
        }
    }
}
