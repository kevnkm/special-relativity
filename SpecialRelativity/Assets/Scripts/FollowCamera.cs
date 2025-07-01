using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;
    public Vector3 offset = new Vector3(0f, 0f, 2f);

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition =
            cameraTransform.position + cameraTransform.TransformDirection(offset);
        desiredPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0;
        if (directionToCamera != Vector3.zero)
        {
            Quaternion desiredRotation =
                Quaternion.LookRotation(directionToCamera, Vector3.up)
                * Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
