using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFlickerFromString : MonoBehaviour
{
    [Header("Flicker Settings")]
    public string flickerPattern = "azazazaz";
    public float flickerSpeed = 1f;

    [Header("Volume Range")]
    public float volumeMin = 0f;
    public float volumeMax = 1f;

    [Header("Phase Offset")]
    public float phaseOffset = 0f;
    public bool randomizePhaseOffset = false;

    private AudioSource source;
    private int patternLength;
    private float actualOffset;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        patternLength = Mathf.Max(1, flickerPattern.Length);
        actualOffset = randomizePhaseOffset ? Random.Range(0f, patternLength) : phaseOffset;
    }

    void Update()
    {
        if (string.IsNullOrEmpty(flickerPattern))
            return;

        // Step through the string based on time, speed, and phase offset
        float t = (Time.time * flickerSpeed + actualOffset) % patternLength;
        int index = Mathf.FloorToInt(t) % patternLength;
        char currentChar = flickerPattern[index];

        // Map 'a' to min, 'z' to max
        float normalized = Mathf.InverseLerp('a', 'z', currentChar);
        float volume = Mathf.Lerp(volumeMin, volumeMax, normalized);
        source.volume = volume;
    }
}
