using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FadeCamera : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private async void Start()
    {
        canvasGroup.alpha = 1f;
        await SetUIFadeTriggerAsync(FadeType.FadeOut);
    }

    public enum FadeType
    {
        FadeIn,
        FadeOut
    }

    public IEnumerator SetUIFadeTrigger(FadeType fadeType, float duration = 1f)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeType == FadeType.FadeIn ? 0f : 1f;
        float targetAlpha = fadeType == FadeType.FadeIn ? 1f : 0f;

        canvasGroup.alpha = startAlpha;
        if (fadeType == FadeType.FadeIn)
            canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            if (canvasGroup == null)
                yield break;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        // Ensure it reaches the exact target value
        canvasGroup.alpha = targetAlpha;

        if (fadeType == FadeType.FadeOut)
            canvasGroup.gameObject.SetActive(false);
    }

    public async Task SetUIFadeTriggerAsync(FadeType fadeType, float duration = 1f)
    {
        await Task.Yield();

        float elapsedTime = 0f;
        float startAlpha = fadeType == FadeType.FadeIn ? 0f : 1f;
        float targetAlpha = fadeType == FadeType.FadeIn ? 1f : 0f;

        canvasGroup.alpha = startAlpha;
        if (fadeType == FadeType.FadeIn)
            canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            if (canvasGroup == null)
                return;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            await Task.Yield();
        }

        // Ensure it reaches the exact target value
        canvasGroup.alpha = targetAlpha;

        if (fadeType == FadeType.FadeOut)
            canvasGroup.gameObject.SetActive(false);
    }
}
