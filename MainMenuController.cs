using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string difficulty = "easy";
    public Button playButton,backButtonPlay, backButtonCredits, backButtonHowto;
    public Dropdown difficultyButton;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void onPlayButton()
    {
        difficultyButton.Select();
    }

    public void onBackButton()
    {
        playButton.Select();
    }

    public void OnCreditsButton()
    {
        backButtonCredits.Select();
    }

    public void OnHowToButton()
    {
        backButtonHowto.Select();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
