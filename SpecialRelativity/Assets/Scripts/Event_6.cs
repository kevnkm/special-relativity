using System.Collections;
using UnityEngine;

public class Event_6 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        var anchor = DialogueManager.Instance.PlatformFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);
        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Hide();
        DialogueManager.Instance.ResetBall();
        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.Train.gameObject.SetActive(false);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 0.7f;

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, -90, 0);

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.EinsteinAnimator.SetTrigger("Right Turn");

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
