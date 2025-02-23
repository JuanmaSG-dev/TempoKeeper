using UnityEngine;
using System.Collections;

public class PetCatMinigame : MonoBehaviour
{
    public GameObject expressionA;
    public GameObject expressionB;
    public GameObject expressionC;
    public GameObject hand;
    private bool canPet = false;
    private bool didPet = false;
    public bool won = false;
    public AudioClip miau;

    void Start()
    {
        StartMinigame();
    }

    public void StartMinigame() {
        StartCoroutine(SwitchExpression());
    }

    IEnumerator SwitchExpression()
    {
        yield return new WaitForSeconds(4f);
        expressionA.SetActive(false);
        expressionB.SetActive(true);
        canPet = true;

        yield return new WaitForSeconds(1f);
        expressionA.SetActive(true);
        expressionB.SetActive(false);
        canPet = false;

        yield return new WaitForSeconds(3f);
        EndMinigame();
    }

    public void EndMinigame()
    {
        if (!won) {
            PenaltyController.Instance.ApplyPenalty();
        }
        MinigameController.Instance.OnMinigameEnd();
    }

    void Update()
    {
        if (!didPet && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)))
        {
            if (canPet)
            {
                PlayCatAudio();
                hand.transform.position = new Vector3(0.3f, 0, 0);
                Debug.Log("Win");
                didPet = true;
                won = true;
            }
            else
            {
                hand.transform.position = new Vector3(0.3f, 0, 0);
                hand.GetComponent<SpriteRenderer>().color = new Color32(255, 129, 129, 255);
                expressionC.SetActive(true);
                Debug.Log("Lose");
                didPet = false;
                won = false;
            }
        }
    }

    void PlayCatAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(miau);
    }
}
