using UnityEngine;
using System.Collections;

public class ConstructionMinigame : MonoBehaviour
{
    public GameObject cementBagPrefab;
    public Transform emptyLeft, npc1Pos, npc2Pos, playerPos, emptyRight;
    public SpriteRenderer npc1Sprite, npc2Sprite, playerSprite;
    public Sprite normalSprite;
    public Sprite holdingSprite;
    public Sprite loseSprite;

    private GameObject cementBag;
    private int currentHolder;
    public bool won = false;
    public AudioClip grunt;

    void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        StartCoroutine(StartPassing());
    }

    IEnumerator StartPassing()
    {
        cementBag = Instantiate(cementBagPrefab, emptyLeft.position, Quaternion.identity);
        cementBag.SetActive(true);
        yield return new WaitForSeconds(1f);

        yield return MoveCement(emptyLeft, npc1Pos);
        yield return new WaitForSeconds(1f);

        yield return MoveCement(npc1Pos, npc2Pos);
        yield return new WaitForSeconds(0.7f);

        yield return MoveCement(npc2Pos, playerPos);
        yield return new WaitForSeconds(0.7f);
    }

    IEnumerator MoveCement(Transform startPos, Transform endPos)
    {
        Vector3 start = startPos.position;
        Vector3 end = endPos.position;

        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            cementBag.transform.position = Vector3.Lerp(start, end, t) + Vector3.up * Mathf.Sin(t * Mathf.PI);
            yield return null;
        }
        PlayGruntAudio();

        if (startPos == emptyLeft) cementBag.SetActive(true);

        if (startPos == npc1Pos) SetCharacterSprite(npc1Sprite, false);
        if (startPos == npc2Pos) SetCharacterSprite(npc2Sprite, false);
        if (startPos == playerPos) SetCharacterSprite(playerSprite, false);

        if (endPos == npc1Pos) SetCharacterSprite(npc1Sprite, true);
        if (endPos == npc2Pos) SetCharacterSprite(npc2Sprite, true);
        if (endPos == playerPos) SetCharacterSprite(playerSprite, true);

        if (endPos == playerPos)
        {
            StartCoroutine(PlayerReactionTime());
        }

        cementBag.transform.position = end;
    }

    IEnumerator PlayerReactionTime()
    {
        float reactionTimeLimit = 0.5f;
        float reactionTimer = reactionTimeLimit;

        while (reactionTimer > 0)
        {
            reactionTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
            {
                Debug.Log("Cement passed!");
                PlayGruntAudio();
                won = true;
                StartCoroutine(MoveCement(playerPos, emptyRight));
                StartCoroutine(WaitGameEnd());
                yield break;
            }

            yield return null;
        }

        Debug.Log("Too slow! You lost.");
        won = false;
        playerSprite.sprite = loseSprite;
        StartCoroutine(WaitGameEnd());
    }

    void SetCharacterSprite(SpriteRenderer spriteRenderer, bool holding)
    {
        spriteRenderer.sprite = holding ? holdingSprite : normalSprite;
    }

    IEnumerator WaitGameEnd()
    {
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }

    public void EndMinigame()
    {
        Destroy(cementBag);
        if (!won) {
            PenaltyController.Instance.ApplyPenalty();
        }
        Debug.Log("Construction Minigame Ended");
        MinigameController.Instance.OnMinigameEnd();
    }

    void PlayGruntAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(grunt);
    }
}
