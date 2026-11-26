using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGameManager : MonoBehaviour
{
    public static LaserGameManager Instance;

    [Header("Scene References")]
    public Transform player;
    public Transform startPoint;
    public Transform playerCamera;
    public LaserSpawner laserSpawner;
    public BonusPanel[] bonusPanels;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Laser State")]
    public bool areLasersDisabled = false;
    public List<GameObject> activeLasers = new List<GameObject>();
    private Coroutine disableCoroutine;

    void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        Debug.Log($"[GAME START] Health: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int amount)
    {
        if (areLasersDisabled) return;

        currentHealth -= amount;
        Debug.Log($"[LASER HIT] -{amount} HP | Current Health: {Mathf.Max(currentHealth, 0)}/{maxHealth}");

        if (playerCamera != null) StartCoroutine(CameraShake(0.15f, 0.12f));

        if (currentHealth <= 0)
        {
            ResetLevel();
        }
    }

    public void ActivateShutoffBonus(float duration = 1.5f)
    {
        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(DisableLasersRoutine(duration));
    }

    private IEnumerator DisableLasersRoutine(float duration)
    {
        areLasersDisabled = true;
        Debug.Log($"[BONUS] Lasers disabled for {duration} seconds!");

        yield return new WaitForSeconds(duration);

        areLasersDisabled = false;
        Debug.Log("[WARNING] Lasers reactivated!");
    }

    public void ResetLevel()
    {
        Debug.Log($"[DEBUG] Moving '{player.name}' from {player.position} to '{startPoint.name}' at {startPoint.position}");

        currentHealth = maxHealth;
        Debug.Log($"[RESPAWN] Health restored: {currentHealth}/{maxHealth}");

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = startPoint.position;
            rb.rotation = startPoint.rotation;
        }

        player.position = startPoint.position;
        player.rotation = startPoint.rotation;

        Physics.SyncTransforms();


        for (int i = activeLasers.Count - 1; i >= 0; i--)
        {
            if (activeLasers[i] != null) Destroy(activeLasers[i]);
        }
        activeLasers.Clear();

        foreach (BonusPanel panel in bonusPanels)
        {
            if (panel != null) panel.ResetPanel();
        }
    }
    public void TriggerWin()
    {
        Debug.Log("[Win] You survived the Laser Corridor!");
        if (laserSpawner != null) laserSpawner.enabled = false;
    }


    private IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = playerCamera.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            playerCamera.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.localPosition = originalPos;
    }
}