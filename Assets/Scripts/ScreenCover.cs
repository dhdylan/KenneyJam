using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenCover : MonoBehaviour
{
    [SerializeField]
    private float fadeInTime = 0.5f;
    [SerializeField]
    private float fadeOutTime = 1.5f;

    private CanvasGroup canvasGroup;
    private Coroutine currentCoroutine = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void Fade(bool fadeIn)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(FadeCoroutine(fadeIn));
    }

    private IEnumerator FadeCoroutine(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        float currentAlpha = canvasGroup.alpha;

        float currentTime = 0.0f;
        float fadeTime = fadeIn ? fadeInTime : fadeOutTime;

        // lerps current time to what it would be based on where currentAlpha currently is
        // that way there is consistent timing if another fade call was made while this one was still executing.
        currentTime = Mathf.Lerp(currentTime, fadeTime, fadeIn ? currentAlpha : 1.0f - currentAlpha);

        while(currentTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, currentTime / fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        currentCoroutine = null;
    }

    public Coroutine GetCurrentCoroutine() { return currentCoroutine; }
}