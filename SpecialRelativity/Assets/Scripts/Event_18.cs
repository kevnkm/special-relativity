using System.Collections;
using UnityEngine;

public class Event_18 : MonoBehaviour
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

        DialogueManager.Instance.Train.transform.position = new Vector3(20f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.25f, 1, 1);

        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(1f, 1, 1);

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, 0, 0);

        // DialogueManager.Instance.PlatformStopwatch.ResetTimer();
        // DialogueManager.Instance.TrainStopwatch.ResetTimer();
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.PlatformButtonObject.SetActive(true);
        DialogueManager.Instance.SignalLightSphereObject.SetActive(true);

        DialogueManager.Instance.Train.gameObject.SetActive(true);

        var anchor = DialogueManager.Instance.PlatformFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 0.7f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        // // einstein right turn animation
        // DialogueManager.Instance.EinsteinAnimator.SetTrigger("Right Turn");

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
