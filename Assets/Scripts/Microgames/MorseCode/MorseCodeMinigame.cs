using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseCodeMinigame : MonoBehaviour, IMinigame
{
    public GameObject dotPrefab;
    public GameObject linePrefab;
    public Transform[] cpuSlots;
    public Transform[] playerSlots;
    
    private List<string> cpuSequence = new List<string>();
    private List<string> playerInput = new List<string>();
    private bool playerCanInput = false;

    void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        StartCoroutine(GenerateCPUSequence());
    }

    IEnumerator GenerateCPUSequence()
{
    cpuSequence.Clear();
    for (int i = 0; i < cpuSlots.Length; i++)
    {
        yield return new WaitForSeconds(0.3f);
        bool isDot = Random.value > 0.5f;
        string symbol = isDot ? "dot" : "line";
        
        if (cpuSequence.Count <= i)
        {
            cpuSequence.Add(symbol);
            Instantiate(isDot ? dotPrefab : linePrefab, cpuSlots[i].position, Quaternion.identity);
        }
    }
    yield return new WaitForSeconds(0.5f); 
    playerCanInput = true;
}

    void Update()
    {
        if (!playerCanInput || playerInput.Count >= playerSlots.Length) return;

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
        {
            RegisterInput("dot");
        }
        else if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetMouseButtonDown(1))
        {
            RegisterInput("line");
        }
    }

    void RegisterInput(string input)
    {
        Instantiate(input == "dot" ? dotPrefab : linePrefab, playerSlots[playerInput.Count].position, Quaternion.identity);
        playerInput.Add(input);

        if (playerInput.Count == cpuSequence.Count)
        {
            StartCoroutine(CheckWinCondition());
        }
    }

    IEnumerator CheckWinCondition()
    {
        yield return new WaitForSeconds(0.5f);
        bool success = true;
        for (int i = 0; i < cpuSequence.Count; i++)
        {
            if (cpuSequence[i] != playerInput[i])
            {
                success = false;
                break;
            }
        }
        if (success)
        {
            Debug.Log("Win!");
        }
        else
        {
            Debug.Log("Lose!");
        }
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("MorseSymbol");
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }

    public void EndMinigame()
    {
        MinigameController.Instance.OnMinigameEnd();
    }
}
