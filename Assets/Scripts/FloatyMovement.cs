using UnityEngine;

public class FloatyMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How far the object drifts from its origin position")]
    public float amplitude = 0.5f;

    [Tooltip("How fast the noise moves through time (lower = lazier, higher = more erratic)")]
    public float frequency = 0.4f;

    [Header("Rotation")]
    [Tooltip("Enable gentle rotation sway")]
    public bool rotationSway = true;

    [Tooltip("Max rotation sway in degrees")]
    public float rotationAmplitude = 15f;

    [Tooltip("Speed of rotation sway")]
    public float rotationFrequency = 0.25f;

    // Unique offsets per axis so each axis moves independently
    private Vector3 _noiseOffset;
    private Vector3 _rotNoiseOffset;
    private Vector3 _originPosition;
    private Quaternion _originRotation;

    void Start()
    {
        _originPosition = transform.localPosition;
        _originRotation = transform.localRotation;

        // Random starting points in the noise field so two objects don't sync up
        _noiseOffset = new Vector3(
            Random.Range(0f, 999f),
            Random.Range(0f, 999f),
            Random.Range(0f, 999f)
        );
        _rotNoiseOffset = new Vector3(
            Random.Range(0f, 999f),
            Random.Range(0f, 999f),
            Random.Range(0f, 999f)
        );
    }

    void Update()
    {
        float t = Time.time * frequency;

        // Sample Perlin noise for each axis — offset heavily so axes are independent
        // Remap from [0,1] to [-1,1] by multiplying by 2 and subtracting 1
        float x = (Mathf.PerlinNoise(_noiseOffset.x + t, _noiseOffset.x + 1.3f) * 2f - 1f) * amplitude;
        float y = (Mathf.PerlinNoise(_noiseOffset.y + t + 31.7f, _noiseOffset.y + 2.9f) * 2f - 1f) * amplitude;
        float z = (Mathf.PerlinNoise(_noiseOffset.z + t + 67.3f, _noiseOffset.z + 4.1f) * 2f - 1f) * amplitude;

        transform.localPosition = _originPosition + new Vector3(x, y, z);

        if (rotationSway)
        {
            float rt = Time.time * rotationFrequency;

            float pitch = (Mathf.PerlinNoise(_rotNoiseOffset.x + rt, _rotNoiseOffset.x + 5.5f) * 2f - 1f) * rotationAmplitude;
            float yaw = (Mathf.PerlinNoise(_rotNoiseOffset.y + rt + 43.1f, _rotNoiseOffset.y + 6.7f) * 2f - 1f) * rotationAmplitude;
            float roll = (Mathf.PerlinNoise(_rotNoiseOffset.z + rt + 81.9f, _rotNoiseOffset.z + 7.3f) * 2f - 1f) * rotationAmplitude;

            transform.localRotation = _originRotation * Quaternion.Euler(pitch, yaw, roll);
        }
    }
}