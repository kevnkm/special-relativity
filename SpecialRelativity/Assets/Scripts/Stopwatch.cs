using System.Collections;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro timeText;

    private void OnEnable()
    {
        timeText.text = "00.00";
    }

    private void OnDisable()
    {
        timeText.text = "00.00";
        StopCount();
    }

    private IEnumerator CountCoroutine()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);
            timeText.text = string.Format("{0:D2}.{1:D2}", seconds, milliseconds);
            yield return null;
        }
    }

    private void StopCount()
    {
        StopCoroutine(CountCoroutine());
    }

    public void ResetTimer()
    {
        timeText.text = "00.00";
    }

    public IEnumerator CountToTime(float targetTime, float duration)
    {
        float elapsed = 0f;
        float startTime = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentTime = Mathf.Lerp(startTime, targetTime, t);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            int milliseconds = Mathf.FloorToInt((currentTime - Mathf.Floor(currentTime)) * 100);
            timeText.text = string.Format("{0:D2}.{1:D2}", seconds, milliseconds);
            yield return null;
        }
    }
}
