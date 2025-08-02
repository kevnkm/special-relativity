using System.Collections;
using UnityEngine;

public class Event_15 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.Train.transform.position = new Vector3(21.5f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(1, 1, 1);

        DialogueManager.Instance.TrainStopwatch.transform.localScale = new Vector3(1, 1, 1);

        //Original Here
        // DialogueManager.Instance.TrainStopwatch.GetComponent<FaceCamera>().enabled = false;

        //Made following modification
        DialogueManager.Instance.TrainStopwatch.GetComponent<FaceCamera>().enabled = true;

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(0.5f, 1, 1);

        DialogueManager.Instance.PlatformStopwatch.ResetTimer();
        DialogueManager.Instance.TrainStopwatch.ResetTimer();
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(true);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(true);

        DialogueManager.Instance.Train.gameObject.SetActive(true);

        var anchor = DialogueManager.Instance.TrainFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 1.6f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        // einstein left turn animation
        DialogueManager.Instance.EinsteinAnimator.SetTrigger("Left Turn");

        StartCoroutine(DialogueManager.Instance.TrainStopwatch.CountToTime(2f, 2f));
        StartCoroutine(DialogueManager.Instance.PlatformStopwatch.CountToTime(1.49f, 2f));

        // StartCoroutine(CloseAndOpenGate());
        yield return StartCoroutine(
            DialogueManager.Instance.MoveEnvironment(new Vector3(14, 0, 0), 2f)
        );

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator CloseAndOpenGate()
    {
        while (DialogueManager.Instance.Environment.transform.position.x < 21.5f)
        {
            yield return null;
        }

        Debug.Log("Closing gate.");
        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));

        Debug.Log("Opening gate.");
        yield return StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));
    }
}
