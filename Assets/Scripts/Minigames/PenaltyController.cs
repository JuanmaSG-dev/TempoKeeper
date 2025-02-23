using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PenaltyController : MonoBehaviour
{
    public static PenaltyController Instance { get; private set; }
    public Image penaltyImage;
    public Image penalty1, penalty2, penalty3;
    public Pendulum pendulum;
    public PendulumGameplay pendulumGameplay;
    public GameObject metronome;
    public MinigameController minigameController;
    public AudioClip penalty;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        penaltyImage.gameObject.SetActive(false);
        penalty1.gameObject.SetActive(false);
        penalty2.gameObject.SetActive(false);
        penalty3.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ApplyPenalty() {
        StartCoroutine(PenaltyTime());
    }

    IEnumerator PenaltyTime() {
        penaltyImage.gameObject.SetActive(true);
        PlayPenaltyAudio();
        int randomPenalty = Random.Range(0, 3);
        switch (randomPenalty) {
            case 0:
                Debug.Log("Penalty: Faster!");
                penalty1.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                penalty1.gameObject.SetActive(false);
                penaltyImage.gameObject.SetActive(false);
                pendulum.tempo += 20f;
                break;
            case 1:
                Debug.Log("Penalty: X2 Crazyness");
                penalty2.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                penalty2.gameObject.SetActive(false);
                penaltyImage.gameObject.SetActive(false);
                pendulumGameplay.SetTwiceCrazynessPenalty(true);
                break;
            case 2:
                Debug.Log("Penalty: No Metronome");
                penalty3.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                penalty3.gameObject.SetActive(false);
                penaltyImage.gameObject.SetActive(false);
                SpriteRenderer sprite = metronome.GetComponent<SpriteRenderer>();
                Color newColor = sprite.color;
                newColor.a = 0f;
                sprite.color = newColor;
                break;
        }
    }

    public void ResetPenalty() {
        pendulum.tempo = 60f;
        pendulumGameplay.SetTwiceCrazynessPenalty(false);
        SpriteRenderer sprite = metronome.GetComponent<SpriteRenderer>();
        Color newColor = sprite.color;
        newColor.a = 1f;
        sprite.color = newColor;
    }

    void PlayPenaltyAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(penalty);
    }
}
