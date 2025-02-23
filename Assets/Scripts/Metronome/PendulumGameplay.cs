using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PendulumGameplay : MonoBehaviour
{
    public Transform pendulum; 
    public float tolerance = 10f;
    public int crazyness = 0;
    private int maxCrazyness = 30;
    private int minCrazyness = 0;
    private bool actionTaken = false; 
    private int lastDirection = 0;
    public AudioClip metronome;
    public Slider slider;
    public Image sliderImage;
    public bool hasStarted = false;
    public DialogueController dialogueController;
    public DialogueController dialogueControllerEnding1;
    public GameObject DialogueEnding1;

    public Image minigameIncoming;
    public GameObject minigameController;
    public MinigameController minigameControllerScript;
    public bool twiceCrazynessPenalty = false;
    public GameObject BlackHole;
    public bool isBoss = false;

    void Start() {
        slider.maxValue = maxCrazyness;
        slider.value = crazyness;
        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (!hasStarted) return;
        float angle = pendulum.eulerAngles.z;
        if (angle > 180f) angle -= 360f;

        bool isInCorrectArea = Mathf.Abs(angle) <= tolerance;
        int currentDirection = angle > 0 ? 1 : -1;

        if (currentDirection != lastDirection)
        {
            lastDirection = currentDirection;
            actionTaken = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            if (isInCorrectArea && !actionTaken)
            {
                crazyness--;
                if (crazyness < minCrazyness) crazyness = minCrazyness;
                PlayAudio();
                //Debug.Log("Correct! Score: " + crazyness);
                actionTaken = true;
            }
        }

        if (!isInCorrectArea && !actionTaken)
        {
            if (twiceCrazynessPenalty) crazyness++;
            crazyness++;
            if (crazyness > maxCrazyness && !isBoss) Ending1(); // Game Over
            //Debug.Log("Missed! Score: " + crazyness);
            actionTaken = true; 
        }

        slider.value = crazyness;
    }

    void PlayAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(metronome);
    }

    IEnumerator StartGame() {
        while (!dialogueController.hasFinished) {
            slider.gameObject.SetActive(false);
            sliderImage.gameObject.SetActive(false);
            yield return null; 
        }
        yield return new WaitForSeconds(1f);
        slider.gameObject.SetActive(true);
        sliderImage.gameObject.SetActive(true);
        hasStarted = true;
        yield return new WaitForSeconds(4f);
        minigameIncoming.gameObject.SetActive(true);
        minigameController.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        minigameIncoming.gameObject.SetActive(false);
    }

    public void SetTwiceCrazynessPenalty(bool penalty) {
        this.twiceCrazynessPenalty = penalty;
    }

    public void Ending1() {
        minigameControllerScript.Ending();
        slider.gameObject.SetActive(false);
        sliderImage.gameObject.SetActive(false);
        hasStarted = false;
        minigameController.gameObject.SetActive(false);
        DialogueEnding1.SetActive(true);
        StartCoroutine(Ending1Dialogue());
    }

    IEnumerator Ending1Dialogue() {
        while (dialogueControllerEnding1.hasFinished == false) {
            yield return null;
        }
        BlackHole.SetActive(true);
    }

}
