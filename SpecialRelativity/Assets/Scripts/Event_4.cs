using System.Collections;
using UnityEngine;

public class Event_4 : MonoBehaviour
{
    [SerializeField]
    private Vector3 ballDropForce = new Vector3(0f, 0.05f, 0.1f);

    [SerializeField]
    private Vector3 environmentPositionDelta = new Vector3(10, 0, 0);

    private void Start()
    {
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);

        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.RelocateTrain(new Vector3(10f, 0f, 0f));

        var anchor = DialogueManager.Instance.TrainFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Hide();
        DialogueManager.Instance.ResetBall();

        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 1.6f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);

        StartCoroutine(EnvironmentAnimationCoroutine());
        StartCoroutine(BallDropCoroutine());
        yield return StartCoroutine(EinsteinAnimationCoroutine());

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator EnvironmentAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(DialogueManager.Instance.MovePlatform(environmentPositionDelta, 2f));
    }

    private IEnumerator BallDropCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Show();
        DialogueManager.Instance.ReleaseBall(ballDropForce);
    }

    private IEnumerator EinsteinAnimationCoroutine()
    {
        var animator = DialogueManager.Instance.TrainEinsteinAnimator;

        animator.SetTrigger("Drop");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float timeout = 5f;
        float elapsed = 0f;
        while ((animator.IsInTransition(0) || !stateInfo.IsName("Drop")) && elapsed < timeout)
        {
            yield return null;
            elapsed += Time.deltaTime;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        float clipLength = stateInfo.length;

        yield return new WaitUntil(
            () =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
                && !animator.IsInTransition(0)
        );

        animator.SetTrigger("Idle");
    }
}
