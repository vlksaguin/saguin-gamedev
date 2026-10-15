using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}