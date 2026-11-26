using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // checkpoints
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    // rotation
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 0f;

    private Vector3 targetPosition;

    void Start()
    {
        if (pointB != null)
        {
            targetPosition = pointB.position;
        }
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
            }
        }

        if (rotationSpeed != 0f)
        {
            transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}