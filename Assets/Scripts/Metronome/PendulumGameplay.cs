using UnityEngine;
using UnityEngine.UI;

public class PendulumGameplay : MonoBehaviour
{
    public Transform pendulum; 
    public float tolerance = 10f;
    private int crazyness = 0;
    private int maxCrazyness = 100;
    private bool missedCycle = false;
    private float time;
    public AudioClip metronome;
    public Slider slider;
    
    void Start() {
        slider.maxValue = maxCrazyness;
        slider.value = crazyness;
    }

    void Update()
    {
        time += Time.deltaTime;

        float angle = pendulum.eulerAngles.z;

        if (angle > 180f) angle -= 360f;

        bool isInCorrectArea = Mathf.Abs(angle) <= tolerance;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            
            if (isInCorrectArea)
            {
                crazyness--;
                PlayAudio();
                Debug.Log("Correct! Score: " + crazyness);
                missedCycle = false;
            }
            else
            {
                crazyness++;
                Debug.Log("Incorrect! Score: " + crazyness);
                missedCycle = false;
            }
        }

        if (isInCorrectArea && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Z) && !missedCycle)
        {
            crazyness++;
            Debug.Log("Missed! Score: " + crazyness);
            missedCycle = true;
        }
        slider.value = crazyness;
    }

    void PlayAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(metronome);
    }


}
