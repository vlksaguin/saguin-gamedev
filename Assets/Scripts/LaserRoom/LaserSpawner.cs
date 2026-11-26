using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public float spawnInterval = 1.1f;
    public float[] spawnHeights = new float[] { 0.4f, 0.8f, 1.0f, 1.2f, 1.6f };

    private float timer;

    void Update()
    {
        if (LaserGameManager.Instance.areLasersDisabled) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnLaser();
            timer = 0f;
        }
    }

    void SpawnLaser()
    {
        float randomHeight = spawnHeights[Random.Range(0, spawnHeights.Length)];
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + randomHeight, transform.position.z);
        Instantiate(laserPrefab, spawnPos, transform.rotation);
    }
}