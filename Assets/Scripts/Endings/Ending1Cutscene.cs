using UnityEngine;

public class Ending1Cutscene : MonoBehaviour
{
    public DialogueController dialogueController;
    public GameObject ending;

    void Start() {

    }

    void Update() {
        if (dialogueController.hasFinished) {
            ending.SetActive(true);
        }
    }
}
