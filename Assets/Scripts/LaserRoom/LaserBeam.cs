using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float speed = 24f;
    public int damage = 35;
    public float lifetime = 5f;
    public GameObject hitSparksPrefab;

    private bool hasHitPlayer = false;
    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private AudioSource laserAudio;
    private ParticleSystem[] wallSparks;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        laserAudio = GetComponent<AudioSource>();
        wallSparks = GetComponentsInChildren<ParticleSystem>();

        LaserGameManager.Instance.activeLasers.Add(gameObject);
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        bool active = !LaserGameManager.Instance.areLasersDisabled;

        if (lineRenderer != null) lineRenderer.enabled = active;
        if (boxCollider != null) boxCollider.enabled = active;
        if (laserAudio != null) laserAudio.enabled = active;

        foreach (ParticleSystem ps in wallSparks)
        {
            var emission = ps.emission;
            emission.enabled = active;
        }

        // Adjust Vector3.back if your corridor runs along a different axis
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer || LaserGameManager.Instance.areLasersDisabled) return;

        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            if (hitSparksPrefab != null)
            {
                GameObject fx = Instantiate(hitSparksPrefab, other.transform.position + Vector3.up, Quaternion.identity);
                Destroy(fx, 1.5f);
            }

            LaserGameManager.Instance.TakeDamage(damage);
        }
    }

    void OnDestroy()
    {
        if (LaserGameManager.Instance != null)
            LaserGameManager.Instance.activeLasers.Remove(gameObject);
    }
}