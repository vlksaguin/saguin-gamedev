using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Color activeColor = Color.green;
    private MeshRenderer meshRenderer;
    private Collider platformCollider;
    private bool isActivated = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

            if (respawn != null)
            {
                // Calculate exact center top using collider bounds
                Vector3 spawnPos;
                if (platformCollider != null)
                {
                    spawnPos = new Vector3(
                        platformCollider.bounds.center.x,
                        platformCollider.bounds.max.y + 1.2f,
                        platformCollider.bounds.center.z
                    );
                }
                else
                {
                    spawnPos = transform.position + Vector3.up * 1.5f;
                }

                respawn.SetCheckpoint(spawnPos);
                isActivated = true;

                if (meshRenderer != null)
                {
                    meshRenderer.material.color = activeColor;
                }
            }
        }
    }
}