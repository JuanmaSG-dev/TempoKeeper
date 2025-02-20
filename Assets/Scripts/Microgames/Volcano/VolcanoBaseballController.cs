using UnityEngine;
using System.Collections;

public class VolcanoBaseballController : MonoBehaviour, IMinigame
{
    public GameObject rockPrefab;
    public Transform spawnPoint;
    public Transform groundPoint;
    public HitZone hitZoneScript;

    public float[] spawnIntervals = { 0.2f, 0.2f, 0.3f, 0.2f };
    public float rockSpeed = 5f;
    public float fallSpeed = 7f;
    public float flyAwaySpeed = 10f;

    private GameObject[] rocks = new GameObject[4];
    private GameObject currentRock;
    public AudioClip hitSound;
    public AudioClip volcanoSound;
    
    public Animator playerAnimator;
    public int hitCounter = 0;


    void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        StartCoroutine(VolcanoPattern());
    }

    public void EndMinigame()
    {
        MinigameController.Instance.OnMinigameEnd();
    }

    IEnumerator VolcanoPattern()
    {
        while (true)
        {

            for (int i = 0; i < spawnIntervals.Length; i++)
            {
                rocks[i] = SpawnRock();
                StartCoroutine(MoveRock(rocks[i], spawnPoint.position + Vector3.up * 3f, rockSpeed));
                yield return new WaitForSeconds(spawnIntervals[i]);
            }

            yield return new WaitForSeconds(2f);

            for (int i = 0; i < spawnIntervals.Length; i++)
            {
                if (rocks[i] != null)
                {
                    currentRock = rocks[i];
                    StartCoroutine(MoveRock(rocks[i], groundPoint.position, fallSpeed));
                }
                yield return new WaitForSeconds(spawnIntervals[i]);
            }
            yield return new WaitForSeconds(3f);
            EndMinigame();
            yield return new WaitForSeconds(2f);
        }
    }

    GameObject SpawnRock()
    {
        GameObject rock = Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
        PlayVolcanoAudio();
        return rock;
    }

    IEnumerator MoveRock(GameObject rock, Vector3 targetPos, float speed)
    {
        while (rock != null && Vector3.Distance(rock.transform.position, targetPos) > 0.1f)
        {
            rock.transform.position = Vector3.MoveTowards(rock.transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        if (rock != null && targetPos == groundPoint.position)
        {
            Destroy(rock, 0.3f);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("Hit");
            GameObject currentRock = hitZoneScript.GetCurrentRock();
            if (currentRock != null)
            {
                Debug.Log("HIT!");
                PlayHitAudio();
                StartCoroutine(FlyAway(currentRock));
            }
            else
            {
                Debug.Log("Miss!");
            }
        }
    }


    IEnumerator FlyAway(GameObject rock)
    {
        Destroy(rock, 0.1f);
        yield break;
    }

    void PlayVolcanoAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(volcanoSound);
    }

    void PlayHitAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);
    }
}
