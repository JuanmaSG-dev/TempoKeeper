using UnityEngine;
using System.Collections;

public class PetCatMinigame : MonoBehaviour, IMinigame
{
    public GameObject expressionA;
    public GameObject expressionB;
    public GameObject expressionC;
    public GameObject hand;
    private bool canPet = false;
    private bool didPet = false;

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
        MinigameController.Instance.OnMinigameEnd();
    }

    void Update()
    {
        if (!didPet && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)))
        {
            if (canPet)
            {
                hand.transform.position = new Vector3(0.3f, 0, 0);
                Debug.Log("Win");
                didPet = true;
            }
            else
            {
                hand.transform.position = new Vector3(0.3f, 0, 0);
                hand.GetComponent<SpriteRenderer>().color = new Color32(255, 129, 129, 255);
                expressionC.SetActive(true);
                Debug.Log("Lose");
                didPet = false;
            }
        }
    }
}
