using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class BossMetronome : MonoBehaviour
{
    public Transform[] movePositions;
    public float moveSpeed = 10f;
    public float moveInterval = 3f;
    public Pendulum pendulum;
    public PendulumGameplay pendulumGameplay;
    private int timer = 60;
    public TextMeshProUGUI timerText;
    public GameObject Metronome;
    public GameObject BrokenMetronome;
    public DialogueController dialogueControllerEnding2;
    public GameObject DialogueEnding2;
    public DialogueController dialogueControllerEnding3;
    public GameObject DialogueEnding3;
    bool isEndingCutsceneTriggered = false;

    void Start()
    {
        StartCoroutine(MoveMetronome());
        StartCoroutine(TimerCountdown());
        pendulum.tempo = 60f;
        pendulumGameplay.isBoss = true;
    }

    IEnumerator MoveMetronome()
    {
        while (timer > 0)
        {
            Transform targetPos = movePositions[Random.Range(0, movePositions.Length)];
            while (Vector3.Distance(Metronome.transform.position, targetPos.position) > 0.1f)
            {
                Metronome.transform.position = Vector3.MoveTowards(Metronome.transform.position, targetPos.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(moveInterval);
            float amount = Random.Range(50f, 80f);
            pendulum.tempo = amount;
        }
    }

    IEnumerator TimerCountdown()
    {
        while (timer >= 0)
        {
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }

    void Update() {
        if (timer == 0 && !isEndingCutsceneTriggered) {
            isEndingCutsceneTriggered = true;
            pendulumGameplay.hasStarted = false;
            pendulumGameplay.StopAllCoroutines();
            Metronome.transform.position = new Vector3(0,-3.5f,0);
            DialogueEnding3.SetActive(true);
            StartCoroutine(Ending3Cutscene());
            // Ending 3
        }
        else if (pendulumGameplay.crazyness == 30 && !isEndingCutsceneTriggered) {
            isEndingCutsceneTriggered = true;
            pendulumGameplay.StopAllCoroutines();
            timerText.gameObject.SetActive(false);
            pendulumGameplay.hasStarted = false;
            Metronome.transform.position = new Vector3(0,-3.5f,0);
            Metronome.gameObject.SetActive(false);
            BrokenMetronome.gameObject.SetActive(true);
            DialogueEnding2.SetActive(true);
            Debug.Log("Ending 2");
            StartCoroutine(Ending2Cutscene());
            // Ending 2
        }
    }

    IEnumerator Ending2Cutscene() {
        while (dialogueControllerEnding2.hasFinished == false) {
            yield return null;
        }
        DialogueEnding2.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Ending2");
    }

    IEnumerator Ending3Cutscene() {
        while (dialogueControllerEnding3.hasFinished == false) {
            yield return null;
        }
        DialogueEnding3.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Ending3");
    }
}