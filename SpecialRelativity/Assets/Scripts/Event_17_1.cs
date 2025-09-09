using System.Collections;
using UnityEngine;

/// <summary>
/// Unfreeze frame, train moves forward,
/// front gate closes and opens,
/// then back gate closes and opens
/// once the back of the train makes it through.
/// </summary>
public class Event_17_1 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        #region Initialize Train Transform
        DialogueManager.Instance.Train.transform.position = new Vector3(11f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(1, 1, 1);
        #endregion

        # region Initialize Environment Transform
        DialogueManager.Instance.Environment.transform.position = new Vector3(6, 0, 0);
        #endregion

        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(GateEventCoroutine());

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator GateEventCoroutine()
    {
        var moveTime = 10f;
        Debug.Log("Right Gate Close by Event17");
        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));
        StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));

        StartCoroutine(DialogueManager.Instance.MoveEnvironment(new Vector3(16, 0, 0), moveTime));

        yield return new WaitForSeconds(moveTime * 0.9f);

        Debug.Log("Left Gate Close by Event17");
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
    }
}
