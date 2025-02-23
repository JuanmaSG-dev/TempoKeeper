using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MinigameController : MonoBehaviour
{
    public static MinigameController Instance { get; private set; }

    public List<GameObject> minigames = new List<GameObject>();
    private GameObject currentMinigame;
    public GameObject MetronomeBoss;

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
            MetronomeBoss.SetActive(true);
            return;
        }

        int index = Random.Range(0, minigames.Count);
        currentMinigame = minigames[index];
        minigames.RemoveAt(index);

        currentMinigame.SetActive(true);
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
        yield return new WaitForSeconds(6f);
        PenaltyController.Instance.ResetPenalty();
        StartNextMinigame();
    }

    public void Ending() {
        minigames.Clear();
        currentMinigame.SetActive(false);
        StopAllCoroutines();
    }
}
