using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private GameObject bar;

    public IEnumerator OpenGate(float duration)
    {
        Quaternion StartRotation = bar.transform.localRotation;
        Quaternion EndRotation = Quaternion.Euler(0, 0, 0);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            bar.transform.localRotation = Quaternion.Slerp(
                StartRotation,
                EndRotation,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        bar.transform.localRotation = EndRotation;
    }

    public IEnumerator CloseGate(float duration)
    {
        Quaternion StartRotation = bar.transform.localRotation;
        Quaternion EndRotation = Quaternion.Euler(0, 90, 0);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            bar.transform.localRotation = Quaternion.Slerp(
                StartRotation,
                EndRotation,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        bar.transform.localRotation = EndRotation;
    }
}
