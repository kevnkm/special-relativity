using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 3f;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
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
