using UnityEngine;

public class BobbingMotion : MonoBehaviour
{
    public enum BobMode { SineWave, PingPong }

    [Header("Bobbing Settings")]
    public Vector3 bobAxis = Vector3.up;     // Axis to bob on (default: Y)
    public float amplitude = 0.25f;          // How far it bobs
    public float frequency = 1f;             // How fast it bobs
    public BobMode motionType = BobMode.SineWave;

    [Header("Phase Offset")]
    public bool randomizeOffset = true;
    public float offset = 0f;                // Manual phase offset (if not randomized)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;

        if (randomizeOffset)
            offset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float time = Time.time * frequency + offset;
        float displacement = 0f;

        switch (motionType)
        {
            case BobMode.SineWave:
                displacement = Mathf.Sin(time) * amplitude;
                break;
            case BobMode.PingPong:
                displacement = (Mathf.PingPong(time, 1f) - 0.5f) * 2f * amplitude;
                break;
        }

        transform.localPosition = startPosition + bobAxis.normalized * displacement;
    }
}