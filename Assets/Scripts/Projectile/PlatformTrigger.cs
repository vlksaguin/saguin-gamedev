using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public enum GameType { Static_Targets, Moving_Targets }

    public GameType gameToSpawn;
    public GameBoothSpawner spawner;
    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.gray;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetColor(inactiveColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawner != null)
        {
            if (gameToSpawn == GameType.Static_Targets)
            {
                spawner.SpawnStaticGame();
            }
            else if (gameToSpawn == GameType.Moving_Targets)
            {
                spawner.SpawnMovingGame();
            }

            SetColor(activeColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetColor(inactiveColor);
        }
    }

    private void SetColor(Color color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
    }
}