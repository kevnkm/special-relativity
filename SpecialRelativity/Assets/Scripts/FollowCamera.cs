using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;
    public Vector3 offset = new Vector3(0f, 0f, 2f); // Offset relative to camera's local space
    public float yPos = 0.5f; // Fixed height offset

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Offset is applied relative to the camera's local space
        Vector3 targetPosition =
            cameraTransform.position
            + cameraTransform.right * offset.x
            + cameraTransform.forward * offset.z;
        targetPosition = new Vector3(targetPosition.x, yPos, targetPosition.z);

        // Smooth position follow
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
