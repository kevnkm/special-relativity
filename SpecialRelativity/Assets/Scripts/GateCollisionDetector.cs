using System.Collections;
using UnityEngine;

public class GateCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private Gate gate;

    public IEnumerator CloseAndOpenGate(float closeDuration = 0.2f, float openDuration = 0.2f)
    {
        yield return StartCoroutine(gate.CloseGate(closeDuration));
        yield return StartCoroutine(gate.OpenGate(openDuration));
    }
}
