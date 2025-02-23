using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameText;
    public string[] lines;
    public float textSpeed;
    private int index;
    public string name1, name2;
    public bool hasFinished = false;
    public AudioClip charAudio;
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (textComponent.text == lines[index]) {
                NextLine();
            } else {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue() {
        hasFinished = false;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        if (index % 2 == 0) {
            nameText.text = name1;
        } else {
            nameText.text = name2;
        }
        /*switch (index) {
            case 0:
                nameText.text = "Mark";
                break;
            case 1:
                nameText.text = "John";
                break;
            case 2:
                nameText.text = "Mark";
                break;
            case 3:
                nameText.text = "John";
                break;
            case 4:
                nameText.text = "Mark";
                break;
            case 5:
                nameText.text = "John";
                break;
            case 6:
                nameText.text = "Mark";
                break;
            case 7:
                nameText.text = "John";
                break;
        }*/
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            PlayCharAudio();
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < lines.Length - 1) 
        { 
            index++; 
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            hasFinished = true;
            gameObject.SetActive(false);
        }
    }

    void PlayCharAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(charAudio);
    }
}
