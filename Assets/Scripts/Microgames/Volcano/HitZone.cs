using UnityEngine;

public class HitZone : MonoBehaviour
{
    private GameObject currentRock;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rock"))
        {
            currentRock = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Rock"))
        {
            currentRock = null;
        }
    }

    public GameObject GetCurrentRock()
    {
        return currentRock;
    }
}
