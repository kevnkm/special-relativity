using System.Collections;
using UnityEngine;

/// <summary>
/// Frame transition to TF, play through the situation again and freeze frame with enough space to close and open the first gate upon resuming.
/// </summary>
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

        #region Initialize Train Transform
        DialogueManager.Instance.Train.transform.position = new Vector3(11f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(1, 1, 1);

        DialogueManager.Instance.TrainStopwatch.transform.localScale = new Vector3(1, 1, 1);

        DialogueManager.Instance.TrainStopwatch.GetComponent<FaceCamera>().enabled = true;

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(0.5f, 1, 1);

        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(true);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(true);

        DialogueManager.Instance.Train.gameObject.SetActive(true);
        #endregion

        #region Locate Player
        var anchor = DialogueManager.Instance.TrainFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 1.6f;
        #endregion

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, -90, 0);

        StartCoroutine(DialogueManager.Instance.TrainStopwatch.CountToTime(2f, 2f));
        StartCoroutine(DialogueManager.Instance.PlatformStopwatch.CountToTime(1.49f, 2f));

        yield return StartCoroutine(
            DialogueManager.Instance.MovePlatform(new Vector3(6, 0, 0), 2f)
        );

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
