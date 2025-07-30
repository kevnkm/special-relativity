using UnityEngine;

public class StopOnCollision : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
    }
}
