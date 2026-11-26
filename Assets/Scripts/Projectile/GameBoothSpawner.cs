using UnityEngine;

public class GameBoothSpawner : MonoBehaviour
{
    public GameObject staticTargetPrefab;
    public GameObject movingTargetPrefab;

    public Transform boothSpawnPoint;

    private GameObject currentActiveGame;

    public void SpawnStaticGame()
    {
        ClearCurrentGame();
        if (staticTargetPrefab != null && boothSpawnPoint != null)
        {
            currentActiveGame = Instantiate(staticTargetPrefab, boothSpawnPoint.position, boothSpawnPoint.rotation);
        }
    }

    public void SpawnMovingGame()
    {
        ClearCurrentGame();
        if (movingTargetPrefab != null && boothSpawnPoint != null)
        {
            currentActiveGame = Instantiate(movingTargetPrefab, boothSpawnPoint.position, boothSpawnPoint.rotation);
        }
    }

    public void ClearCurrentGame()
{
    if (currentActiveGame != null)
    {
        Destroy(currentActiveGame);
    }

    // cleanup for stray objects tagged "Target"
    GameObject[] strayTargets = GameObject.FindGameObjectsWithTag("Target");
    foreach (GameObject t in strayTargets)
    {
        Destroy(t);
    }
}
}