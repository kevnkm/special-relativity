using System.Collections;
using UnityEngine;

public class GateCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private Gate gate;

    public IEnumerator CloseAndOpenGate(float closeDuration, float openDuration)
    {
        yield return StartCoroutine(gate.CloseGate(closeDuration));
        yield return StartCoroutine(gate.OpenGate(openDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SignalLightSphere"))
        {
            Debug.Log($"LightSphere collided with gate: {gameObject.name}");
            StartCoroutine(CloseAndOpenGate(0.2f, 0.2f));
        }
    }
}
