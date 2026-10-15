using UnityEngine;

public class TargetKnockdown : MonoBehaviour
{
    public float knockForce = 8f;
    public float flipTorque = 15f;

    public bool isGroupTarget = false;
    private Rigidbody rb;
    private MovingPlatform movingScript;
    private bool isKnockedDown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movingScript = GetComponent<MovingPlatform>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isKnockedDown)
        {
            isKnockedDown = true;

            // Stop movement script on this target
            if (movingScript != null)
            {
                movingScript.enabled = false;
            }

            if (isGroupTarget && transform.parent != null)
            {
                Rigidbody[] groupMembers = transform.parent.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody memRb in groupMembers){
                    memRb.isKinematic = false;
                    memRb.useGravity = true;
                }
                
            }
            else if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            // physics while keeping object in the parent hierarchy
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce((hitDirection + Vector3.down * 0.2f) * knockForce, ForceMode.Impulse);
                rb.AddTorque(transform.right * flipTorque, ForceMode.Impulse);
            }
        }
    }
}