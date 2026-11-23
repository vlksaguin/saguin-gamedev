using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserDetector : MonoBehaviour
{
    [Header("Laser Endpoints")]
    public Transform startPoint;
    public Transform endPoint;
    public float laserRadius = 0.25f;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    void FixedUpdate()
    {
        if (startPoint == null || endPoint == null) return;

        // 1. OVERLAP CHECK
        Collider[] hits = Physics.OverlapCapsule(startPoint.position, endPoint.position, laserRadius, playerLayer);

        foreach (Collider playerCol in hits)
        {
            // Set target point to the player's waist
            Vector3 playerCenter = playerCol.bounds.center - new Vector3(0, 0.4f, 0); 
            
            // Get the exact closest spot on the rotating laser beam
            Vector3 closestPointOnLaser = ClosestPointOnLine(startPoint.position, endPoint.position, playerCenter);

            // DIRECTION: FROM LASER ---> TO PLAYER
            Vector3 directionToPlayer = (playerCenter - closestPointOnLaser).normalized;
            float distanceToPlayer = Vector3.Distance(closestPointOnLaser, playerCenter);

            // Combine masks so the ray stops at whichever it hits first (wall or player)
            LayerMask combinedMask = playerLayer | obstacleLayer;

            // 2. RAYCAST: FROM LASER ---> TO PLAYER
            if (Physics.Raycast(closestPointOnLaser, directionToPlayer, out RaycastHit hit, distanceToPlayer + 0.5f, combinedMask))
            {
                if (hit.collider.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
                {
                    // CAUGHT! Ray reached the player without hitting a wall.
                    Debug.DrawRay(closestPointOnLaser, directionToPlayer * hit.distance, Color.red, 2f);
                    Debug.Log("Laser Detected Player!");
                    
                    // Uncomment to re-enable scene reset
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    // SAFE! Ray hit a wall first.
                    Debug.DrawRay(closestPointOnLaser, directionToPlayer * hit.distance, Color.green, 2f);
                }
            }
        }
    }

    private Vector3 ClosestPointOnLine(Vector3 a, Vector3 b, Vector3 point)
    {
        Vector3 ab = b - a;
        float t = Mathf.Clamp01(Vector3.Dot(point - a, ab) / Vector3.Dot(ab, ab));
        return a + ab * t;
    }
}