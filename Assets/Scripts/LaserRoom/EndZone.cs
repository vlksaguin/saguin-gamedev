using UnityEngine;

public class EndZone : MonoBehaviour
{
    private bool finished = false;

    void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            finished = true;
            LaserGameManager.Instance.TriggerWin();
        }
    }
}