using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject KittyContainer, hudContainer, GameOverPanel,newHighscore;
    private int Lives, WaveNbr;
    public int countDownTime;
    public float Score, ScoreIncr;
    public Text LivesCounter, ScoreCounter, WaveCounter, countDownText;
    public bool gamePlaying { get; private set; }

    private GameObject characterChoiceObject;
    public string CharacterChoice;
    public string DifficultyChoice;
    public GameObject smallDog, mediumDog, largeDog, easyWave, mediumWave, hardWave;

    public Button restartButton;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Score = 0;
        ScoreCounter.text = "Score: " + Score;

        Lives = KittyContainer.transform.childCount;
        LivesCounter.text = "Kitties left: " + Lives;

        WaveNbr = 0;
        WaveCounter.text = "Wave " + WaveNbr;

        gamePlaying = false;
        StartCoroutine(countDownToStart());

        characterChoiceObject = GameObject.FindWithTag("CharacterChoice");
        CharacterChoice = characterChoiceObject.GetComponent<CharacterChoice>().dogType;
        DifficultyChoice = characterChoiceObject.GetComponent<CharacterChoice>().difficulty;
        SetUpDog();
        SetUpDifficulty();
    }
    private void SetUpDog()
    {
        if(CharacterChoice == "small")
        {
            smallDog.SetActive(true);
            mediumDog.SetActive(false);
            largeDog.SetActive(false);
        }
        else if (CharacterChoice == "medium")
        {
            smallDog.SetActive(false);
            mediumDog.SetActive(true);
            largeDog.SetActive(false);
        }
        else if (CharacterChoice == "large")
        {
            smallDog.SetActive(false);
            mediumDog.SetActive(false);
            largeDog.SetActive(true);
        }
    }

    private void SetUpDifficulty()
    {
        if (DifficultyChoice == "Easy")
        {
            easyWave.SetActive(true);
            mediumWave.SetActive(false);
            hardWave.SetActive(false);
            GameOverPanel.transform.Find("Highscore").GetComponent<Text>().text = "" + PlayerPrefs.GetInt("easyHighscore", 0);
        }
        else if (DifficultyChoice == "Medium")
        {
            easyWave.SetActive(false);
            mediumWave.SetActive(true);
            hardWave.SetActive(false);
            GameOverPanel.transform.Find("Highscore").GetComponent<Text>().text = "" + PlayerPrefs.GetInt("mediumHighscore", 0);
        }
        else if (DifficultyChoice == "Hard")
        {
            easyWave.SetActive(false);
            mediumWave.SetActive(false);
            hardWave.SetActive(true);
            GameOverPanel.transform.Find("Highscore").GetComponent<Text>().text = "" + PlayerPrefs.GetInt("hardHighscore", 0);
        }
    }
    private void beginGame()
    {
        gamePlaying = true;
    }
    private void Update()
    {
        if(gamePlaying)
        {
            Score += ScoreIncr * Time.deltaTime;
            ScoreCounter.text = "Score: " + (int)Score;
        }
    }

    public void DefeatEnemy()
    {
        Score += 100;
        ScoreCounter.text = "Score: " + (int)Score;
    }

    public void LoseKitty()
    {
        Lives--;
        LivesCounter.text = "Kitties left: " + Lives;
        if (Lives <= 0)
        {
            EndGame();
        }
    }
    public void IncrWave()
    {
        WaveNbr++;
        WaveCounter.text = "Wave " + WaveNbr;
    }

    private void EndGame()
    {
        gamePlaying = false;
        Invoke("ShowGameOverScreen", 1.25f);        
    }

    private void ShowGameOverScreen()
    {
        GameOverPanel.SetActive(true);
        hudContainer.SetActive(false);
        if(Score > PlayerPrefs.GetInt(DifficultyChoice + "Highscore", 0))
        {
            newHighscore.SetActive(true);
            PlayerPrefs.SetInt(DifficultyChoice + "Highscore", (int)Score);
            GameOverPanel.transform.Find("Highscore").GetComponent<Text>().text = "" + PlayerPrefs.GetInt(DifficultyChoice + "Highscore", 0);
        }
        GameOverPanel.transform.Find("HighScoreText").GetComponent<Text>().text = DifficultyChoice + " Highscore";
        GameOverPanel.transform.Find("Score").GetComponent<Text>().text = "" + (int)Score;
        restartButton.Select();
        
    }

    IEnumerator countDownToStart()
    {
        while(countDownTime > 0)
        {
            countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        beginGame();
        countDownText.text = "GO";

        yield return new WaitForSeconds(1f);

        countDownText.gameObject.SetActive(false);
    }

    public void tryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void resetHighscore()
    {

        PlayerPrefs.DeleteKey(DifficultyChoice + "Highscore");
        GameOverPanel.transform.Find("Highscore").GetComponent<Text>().text = "" + PlayerPrefs.GetInt(DifficultyChoice + "Highscore");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(GameObject.FindWithTag("CharacterChoice"));
    }
}
