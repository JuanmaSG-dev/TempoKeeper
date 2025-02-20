using UnityEngine;
using UnityEngine.UI;

public class PendulumGameplay : MonoBehaviour
{
    public Transform pendulum; 
    public float tolerance = 10f;
    private int crazyness = 0;
    private int maxCrazyness = 100;
    private int minCrazyness = 0;
    private bool actionTaken = false; 
    private int lastDirection = 0;
    public AudioClip metronome;
    public Slider slider;

    void Start() {
        slider.maxValue = maxCrazyness;
        slider.value = crazyness;
    }

    void Update()
    {
        float angle = pendulum.eulerAngles.z;
        if (angle > 180f) angle -= 360f;

        bool isInCorrectArea = Mathf.Abs(angle) <= tolerance;
        int currentDirection = angle > 0 ? 1 : -1;

        if (currentDirection != lastDirection)
        {
            lastDirection = currentDirection;
            actionTaken = false;
        }

        if (isInCorrectArea && !actionTaken)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
            {
                crazyness--;
                if (crazyness < minCrazyness) crazyness = minCrazyness;
                PlayAudio();
                Debug.Log("Correct! Score: " + crazyness);
                actionTaken = true;
            }
        }

        if (!isInCorrectArea && !actionTaken)
        {
            crazyness++;
            if (crazyness > maxCrazyness) crazyness = maxCrazyness; // Game Over
            Debug.Log("Missed! Score: " + crazyness);
            actionTaken = true; 
        }

        slider.value = crazyness;
    }

    void PlayAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(metronome);
    }
}
