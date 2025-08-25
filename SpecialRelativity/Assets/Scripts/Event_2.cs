using System.Collections;
using UnityEngine;

public class Event_2 : MonoBehaviour
{
    [SerializeField]
    private Vector3 ballDropForce = new Vector3(-0.01f, 0.05f, 0.1f);

    [SerializeField]
    private Vector3 trainPositionDelta = new Vector3(-10, 0, 0);

    [SerializeField]
    private AnimationClip einsteinDropAnimation;

    private void Start()
    {
        DialogueManager.Instance.EinsteinOnTrain.SetActive(true);
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return StartCoroutine(InitializeTrain());

        StartCoroutine(TrainAnimationCoroutine());
        StartCoroutine(BallDropCoroutine());
        yield return StartCoroutine(EinsteinAnimationCoroutine());

        yield return new WaitForSeconds(1f);
        Debug.Log($"Event 2 completed.");
        DialogueManager.Instance.StartNextNode();
        // DialogueManager.Instance.UserResponse.SetActive(true);
        Destroy(gameObject);
    }

    private IEnumerator BallDropCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Show();
        DialogueManager.Instance.ReleaseBall(ballDropForce);
    }

    private IEnumerator InitializeTrain()
    {
        DialogueManager.Instance.Train.transform.position = new Vector3(60f, -0.57f, -1.32f);
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(-52f, 0, 0), 2f)
        );
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

        // Wait for the duration of the "Drop" animation.
        yield return new WaitForSeconds(einsteinDropAnimation.length);

        animator.SetTrigger("Idle");
    }
}
