using UnityEngine;

public class BonusPanel : MonoBehaviour
{
    public float disableDuration = 1.5f;
    public ParticleSystem activationParticles;
    private bool isUsed = false;
    private Renderer rend;
    private Color originalColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isUsed) return;

        if (other.CompareTag("Player"))
        {
            isUsed = true;
            LaserGameManager.Instance.ActivateShutoffBonus(disableDuration);

            if (activationParticles != null)
                activationParticles.Play();

            if (rend != null)
            {
                rend.material.color = Color.gray;
                rend.material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void ResetPanel()
    {
        isUsed = false;
        if (rend != null)
        {
            rend.material.color = originalColor;
            rend.material.EnableKeyword("_EMISSION");
        }
    }
}