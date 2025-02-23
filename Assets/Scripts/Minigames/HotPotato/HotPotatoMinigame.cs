using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotPotatoMinigame : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform[] positions; // The player is number 0.
    private int currentHolder;
    private GameObject bomb;
    private int passCount = 0;
    public int maxPasses = 10;
    private bool playerCanPass = false;
    public bool won = false;
    public AudioClip boom;
    public AudioClip pass;
    
    void Start() {
        StartMinigame();
    }
    public void StartMinigame()
    {
        currentHolder = Random.Range(0, positions.Length);
        bomb = Instantiate(bombPrefab, new Vector3(positions[currentHolder].position.x, 5f, 0), Quaternion.identity);
        StartCoroutine(DropBomb());
    }
    
    IEnumerator DropBomb()
    {
        Vector3 start = bomb.transform.position;
        Vector3 end = positions[currentHolder].position;
        float duration = 0.5f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            bomb.transform.position = Vector3.Lerp(start, end, t) + Vector3.up * Mathf.Sin(t * Mathf.PI);
            yield return null;
        }
        
        if (currentHolder == 0)
        {
            playerCanPass = true;
        }
        else
        {
            StartCoroutine(NPCTossBomb());
        }
    }

    IEnumerator NPCTossBomb()
    {
        yield return new WaitForSeconds(0.4f);
        PassBombToRandom();
    }
    
    void Update()
    {
        if (playerCanPass && (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0)))
        {
            playerCanPass = false;
            PassBombToRandom();
        }
    }
    
    void PassBombToRandom()
    {
        if (passCount >= maxPasses)
        {
            Explode();
        } else {
            int newHolder;
            do
            {
                newHolder = Random.Range(0, positions.Length);
            } while (newHolder == currentHolder);
            
            currentHolder = newHolder;
            passCount++;
            
            
            StartCoroutine(MoveBombToNewHolder());
        }
    }
    
    IEnumerator MoveBombToNewHolder()
    {
        Vector3 start = bomb.transform.position;
        Vector3 end = positions[currentHolder].position;
        float duration = 0.4f;
        float time = 0;
        PlayPassAudio();

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            bomb.transform.position = Vector3.Lerp(start, end, t) + Vector3.up * Mathf.Sin(t * Mathf.PI);
            yield return null;
        }
        
        if (currentHolder == 0)
        {
            playerCanPass = true;
        }
        else
        {
            StartCoroutine(NPCTossBomb());
        }
    }
    
    void Explode()
    {
        PlayBoomAudio();
        Debug.Log("Boom! " + (currentHolder == 0 ? "Player Loses" : "NPC Eliminated"));
        if (currentHolder == 0) {
            won = false;
        } else {
            won = true;
        }
        Destroy(bomb);
        EndMinigame();
    }
    
    public void EndMinigame()
    {
        if (!won) {
            PenaltyController.Instance.ApplyPenalty();
        }
        Debug.Log("Hot Potato Minigame Ended");
        MinigameController.Instance.OnMinigameEnd();
    }

    void PlayPassAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(pass);
    }

    void PlayBoomAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(boom);
    }
}
