using System.Collections;
using UnityEngine;

public class Event_20 : MonoBehaviour
{
    private TrainCollisionDetector listener;

    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);

        Debug.Log($"{gameObject.name} started.");

        listener =
            DialogueManager.Instance.TrainCollider.gameObject.GetComponent<TrainCollisionDetector>();
        listener.OnTriggerEntered += HandleTrainTrigger;

        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.Train.transform.position = new Vector3(12f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(1f, 1, 1);

        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(0.5f, 1, 1);

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, -90, 0);

        // DialogueManager.Instance.PlatformStopwatch.ResetTimer();
        // DialogueManager.Instance.TrainStopwatch.ResetTimer();
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.PlatformButtonObject.SetActive(true);
        DialogueManager.Instance.SignalLightSphereObject.SetActive(true);

        DialogueManager.Instance.TrainFrameAnchor.transform.position = new Vector3(
            DialogueManager.Instance.TrainFrameAnchor.transform.position.x - 4f,
            DialogueManager.Instance.TrainFrameAnchor.transform.position.y,
            DialogueManager.Instance.TrainFrameAnchor.transform.position.z
        );

        DialogueManager.Instance.TrainFrameAnchor.transform.localRotation = Quaternion.Euler(
            0,
            -90,
            0
        );

        DialogueManager.Instance.SignalLightSphere.Reset();
        DialogueManager.Instance.PlatformButtonLightSphere.Reset();

        var anchor = DialogueManager.Instance.TrainFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 1.6f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(3f);

        ///

        StartCoroutine(WaitForThreshold());

        yield return StartCoroutine(
            DialogueManager.Instance.MoveEnvironment(new Vector3(28f, 0, 0f), 12f)
        );

        ///

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

        anchor = DialogueManager.Instance.PlatformFrameAnchor;
        teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        DialogueManager.Instance.UserResponse.GetComponent<FollowCamera>().yPos = 0.7f;

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        ///

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private void HandleTrainTrigger(Collider other)
    {
        Debug.Log($"Train collided with: {other.name}");
        DialogueManager.Instance.PlatformButtonLightSphere.TriggerScale(
            new Vector3(100, 100, 100),
            8f
        );
        listener.OnTriggerEntered -= HandleTrainTrigger;
    }

    private IEnumerator WaitForThreshold()
    {
        while (DialogueManager.Instance.Platform.transform.position.x < 25f)
        {
            yield return null;
        }

        yield return StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
    }

    private void OnDestroy()
    {
        if (listener != null)
            listener.OnTriggerEntered -= HandleTrainTrigger;
    }
}
