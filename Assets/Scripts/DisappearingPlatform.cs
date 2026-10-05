using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearDelay = 0.5f;
    public float respawnDelay = 3f;

    private MeshRenderer meshRenderer;
    private Collider platformCollider;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(VanishSequence());
        }
    }

    IEnumerator VanishSequence()
    {
        yield return new WaitForSeconds(disappearDelay);

        meshRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        meshRenderer.enabled = true;
        platformCollider.enabled = true;

    }
}