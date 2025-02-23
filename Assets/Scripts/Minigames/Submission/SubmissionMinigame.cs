using UnityEngine;
using System.Collections;
using TMPro;

public class SubmissionMinigame : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private int timer = 10;
    private bool gameOver = false;
    public TextMeshProUGUI submitText;
    public bool won = false;
    public AudioClip submit;

    void Start()
    {
        StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        while (timer >= 0)
        {
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }

        GameOver(false);
    }

    void Update()
    {
        if (!gameOver && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)))
        {
            GameOver(true);
        }
    }

    void GameOver(bool victory)
    {
        gameOver = true;
        if (victory)
        {
            submitText.text = "Submitted!";
            PlaySubmitAudio();
            won = true;
            Debug.Log("Win!");
        }
        else
        {
            submitText.text = "Too late!";
            won = false;
            Debug.Log("Lose!");
        }
        StartCoroutine(WaitGameEnd());  
    }

    IEnumerator WaitGameEnd()
    {
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }

    public void EndMinigame()
    {
        if (!won) {
            PenaltyController.Instance.ApplyPenalty();
        }
        MinigameController.Instance.OnMinigameEnd();
    }

    void PlaySubmitAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(submit);
    }
}
