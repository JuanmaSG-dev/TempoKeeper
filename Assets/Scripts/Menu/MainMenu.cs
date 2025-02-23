using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Intro");
    }


    public void SalirButton()
    {
        Application.Quit();
    }

    public void VolverButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryButton() {
        SceneManager.LoadScene("Game");
    }
}
