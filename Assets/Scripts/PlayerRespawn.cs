using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // respawn
    public float killYThreshold = -30f;
    private Vector3 currentCP;
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
        currentCP = transform.position;
    }

    void Update()
    {
        if (transform.position.y < killYThreshold)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = currentCP;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetCheckpoint(Vector3 newCPPosition)
    {
        currentCP = newCPPosition;
        Debug.Log("Checkpoint Saved: " + newCPPosition);
    }
}