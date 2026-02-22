using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Light2DIntensityPattern : MonoBehaviour
{
    [Header("Pattern Settings")]
    [Tooltip("String pattern using a-z to represent intensity from min to max.")]
    public string pattern = "abcdefghihgfedcba";
    public float stepTime = 0.1f; // Time between steps (in seconds)
    public bool loop = true;

    [Header("Intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;

    private Light2D light2D;
    private float timer = 0f;
    private int index = 0;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (string.IsNullOrEmpty(pattern))
            return;

        timer += Time.deltaTime;
        if (timer >= stepTime)
        {
            timer -= stepTime;
            ApplyCurrentChar();
            AdvanceIndex();
        }
    }

    void ApplyCurrentChar()
    {
        char c = char.ToLower(pattern[index]);
        float t = Mathf.InverseLerp('a', 'z', c);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        light2D.intensity = intensity;
    }

    void AdvanceIndex()
    {
        index++;
        if (index >= pattern.Length)
        {
            if (loop)
                index = 0;
            else
                enabled = false; // Stop updating if not looping
        }
    }
}
