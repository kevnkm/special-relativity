using System.Collections;
using UnityEngine;

public class Event_2 : MonoBehaviour
{
    [SerializeField]
    private Vector3 ballDropForce = new Vector3(-0.01f, 0.05f, 0.1f);

    [SerializeField]
    private Vector3 trainPositionDelta = new Vector3(-10, 0, 0);

    private void Start()
    {
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        StartCoroutine(TrainAnimationCoroutine());
        StartCoroutine(BallDropCoroutine());
        yield return StartCoroutine(EinsteinAnimationCoroutine());

        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator BallDropCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        DialogueManager.Instance.ReleaseBall(ballDropForce);
    }

    private IEnumerator TrainAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(DialogueManager.Instance.MoveTrain(trainPositionDelta, 2f));
    }

    private IEnumerator EinsteinAnimationCoroutine()
    {
        var animator = DialogueManager.Instance.TrainEinsteinAnimator;

        animator.SetTrigger("Drop");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (animator.IsInTransition(0) || !stateInfo.IsName("Drop"))
        {
            yield return null;
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
