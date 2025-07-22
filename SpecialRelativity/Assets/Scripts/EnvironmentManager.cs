using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gateLevers;

    private void Start()
    {
        for (int i = 0; i < gateLevers.Length; i++)
            StartCoroutine(DeactivateLever(i));
    }

    private IEnumerator DeactivateLever(int index = 0)
    {
        if (index < 0 || index >= gateLevers.Length)
        {
            Debug.LogError("Index out of bounds for gate levers.");
            yield break;
        }

        float duration = 1f;
        float elapsed = 0f;
        Quaternion startRotation = gateLevers[index].transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 0);

        while (elapsed < duration)
        {
            gateLevers[index].transform.localRotation = Quaternion.Slerp(
                startRotation,
                endRotation,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        gateLevers[index].transform.localRotation = endRotation;
    }
}
