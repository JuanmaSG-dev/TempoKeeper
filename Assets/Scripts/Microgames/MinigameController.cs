using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MinigameController : MonoBehaviour
{
    public static MinigameController Instance { get; private set; }

    public List<GameObject> minigames = new List<GameObject>();
    private GameObject currentMinigame;

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

    private void Start()
    {
        StartCoroutine(WaitingTime());
    }

    public void StartNextMinigame()
    {
        if (minigames.Count == 0)
        {
            Debug.Log("Todos los minijuegos han terminado.");
            return;
        }

        int index = Random.Range(0, minigames.Count);
        currentMinigame = minigames[index];
        minigames.RemoveAt(index);

        currentMinigame.SetActive(true);

        IMinigame minigameScript = currentMinigame.GetComponent<IMinigame>();
        if (minigameScript != null)
        {
            minigameScript.StartMinigame();
        }
    }

    public void OnMinigameEnd()
    {
        if (currentMinigame != null)
        {
            currentMinigame.SetActive(false);
        }
        StartCoroutine(WaitingTime());
    }

    IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(4f);
        StartNextMinigame();
    }
}
