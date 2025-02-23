using UnityEngine;
using System.Collections;

public class GuitarHeroMinigame : MonoBehaviour
{
    public GameObject piecePrefab;
    private GameObject piece;
    public Transform[] positions; // The player is number 2.
    public Transform[] spawns;
    public int pieceSpeed = 5;
    public int totalPieces = 12;
    public HitZoneGuitar hitZone;
    
    private int spawnedPieces = 0;
    public int hitCounter = 0;
    public int goodCounter = 0;
    public int badCounter = 0;
    
    public AudioClip guitar;
    void Start()
    {
        StartCoroutine(SpawnPieces());
    }

    IEnumerator SpawnPieces()
    {
        while (spawnedPieces < totalPieces)
        {
            int lane = Random.Range(0, spawns.Length);
            piece = Instantiate(piecePrefab, spawns[lane].position, Quaternion.identity);
            StartCoroutine(MovePiece(piece, positions[lane].position));
            spawnedPieces++;
            yield return new WaitForSeconds(0.7f);
        }
    }

    IEnumerator MovePiece(GameObject piece, Vector3 target)
    {
        while (piece != null && Vector3.Distance(piece.transform.position, target) > 0.1f)
        {
            piece.transform.position = Vector3.MoveTowards(piece.transform.position, target, pieceSpeed * Time.deltaTime);
            yield return null;
        }
        hitCounter++;
        yield return new WaitForSeconds(0.2f);
        if (piece != null)
        {
            PlayGuitarAudio();
            if (target == positions[2].position) {
                badCounter++;
            }
            Debug.Log("Piece destroyed!");
            Destroy(piece);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
        {
            GameObject currentPiece = hitZone.GetCurrentPiece();
            if (currentPiece != null)
            {
                PlayGuitarAudio();
                Debug.Log("Hit!");
                goodCounter++;
                Destroy(currentPiece);
            }
            else
            {
                badCounter++;
                Debug.Log("Miss!");
            }
        }
        if (hitCounter == totalPieces) {
            StartCoroutine(WaitGameEnd());
        }
    }

    IEnumerator WaitGameEnd()
    {
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }

    public void EndMinigame()
    {
        Destroy(piece);
        if (badCounter > goodCounter) {
            PenaltyController.Instance.ApplyPenalty();
        }
        Debug.Log("Guitar Hero Minigame Ended");
        MinigameController.Instance.OnMinigameEnd();
    }

    void PlayGuitarAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(guitar);
    }
}
