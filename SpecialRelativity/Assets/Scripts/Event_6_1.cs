using System.Collections;
using UnityEngine;

public class Event_6_1 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        var anchor = DialogueManager.Instance.PlatformFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);
        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Hide();
        DialogueManager.Instance.ResetBall();
        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.Train.gameObject.SetActive(false);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 0.7f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        // next node
        DialogueManager.Instance.StartNextNode();
    }
}
