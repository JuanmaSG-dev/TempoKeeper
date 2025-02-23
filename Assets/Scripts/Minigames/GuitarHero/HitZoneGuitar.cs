using UnityEngine;

public class HitZoneGuitar : MonoBehaviour
{
    private GameObject currentPiece;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Piece"))
        {
            currentPiece = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Piece"))
        {
            currentPiece = null;
        }
    }

    public GameObject GetCurrentPiece()
    {
        return currentPiece;
    }
}
