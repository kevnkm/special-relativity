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
        StartCoroutine(CountCoroutine());
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
}
