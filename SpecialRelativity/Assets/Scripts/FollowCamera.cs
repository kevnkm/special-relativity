using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;
    public Vector3 offset = new Vector3(0f, 0f, 2f); // Offset relative to camera's local space

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Flattened forward and right to avoid Y tilt
        Vector3 flatForward = new Vector3(
            cameraTransform.forward.x,
            0f,
            cameraTransform.forward.z
        ).normalized;
        Vector3 flatRight = new Vector3(
            cameraTransform.right.x,
            0f,
            cameraTransform.right.z
        ).normalized;

        // Apply offset in flattened local space
        Vector3 targetPosition =
            cameraTransform.position + flatRight * offset.x + flatForward * offset.z;

        // Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
