using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform cameraTransform;
    public float launchForce = 35f;
    
    public Vector3 spawnOffset = new Vector3(0f, -0.1f, 1.2f); 

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("[Shooter] Left Click Detected!");
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("[Shooter Error] 'Projectile Prefab' slot is empty in Inspector!");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("[Shooter Error] 'Camera Transform' slot is empty in Inspector!");
            return;
        }

        // compute spawn position safely in front of camera
        Vector3 spawnPosition = cameraTransform.position + 
                                cameraTransform.right * spawnOffset.x + 
                                cameraTransform.up * spawnOffset.y + 
                                cameraTransform.forward * spawnOffset.z;

        // create ball
        GameObject ballInstance = Instantiate(projectilePrefab, spawnPosition, cameraTransform.rotation);

        // apply force
        Rigidbody rb = ballInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // reset any initial velocity
            rb.AddForce(cameraTransform.forward * launchForce, ForceMode.Impulse);
            Debug.Log("[Shooter] Projectile Fired Successfully!");
        }
        else
        {
            Debug.LogError("[Shooter Error] Your Ball Prefab is missing a Rigidbody component!");
        }
    }
}