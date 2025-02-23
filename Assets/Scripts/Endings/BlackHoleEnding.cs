using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlackHoleEnding : MonoBehaviour
{
    public GameObject blackHole;
    public float growDuration = 2f;
    public Vector3 finalSize = new Vector3(5f, 5f, 1f);
    public AudioClip nova;

    void Start() {
        StartCoroutine(GrowBlackHole());
    }
    IEnumerator GrowBlackHole()
    {
        blackHole.SetActive(true);
        Vector3 startSize = blackHole.transform.localScale;
        float timer = 0f;

        PlayNovaAudio();
        while (timer < growDuration)
        {
            blackHole.transform.localScale = Vector3.Lerp(startSize, finalSize, timer / growDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        blackHole.transform.localScale = finalSize;
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Ending1");
    }

    void PlayNovaAudio() {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(nova);
    }
}
