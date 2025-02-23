using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroFinish : MonoBehaviour
{
    public DialogueController dialogueController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (dialogueController.hasFinished) {
            StartCoroutine(WaitGameEnd());
        }
    }

    IEnumerator WaitGameEnd()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
