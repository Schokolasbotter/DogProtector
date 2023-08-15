using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoice : MonoBehaviour
{
    public GameObject menumanager;
    public string dogType = "medium";
    public string difficulty = "easy";
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void setDogType(int dogTypeIndex)
    {
        Debug.Log(dogTypeIndex);
        if (dogTypeIndex == 0)
        {
            dogType = "small";
        }
        else if (dogTypeIndex == 1)
        {
            dogType = "medium";
        }
        if (dogTypeIndex == 2)
        {
            dogType = "large";
        }
    }

    public void setDifficulty(int difficultyIndex)
    {
        if (difficultyIndex == 0)
        {
            difficulty = "Easy";
        }
        else if (difficultyIndex == 1)
        {
            difficulty = "Medium";
        }
        if (difficultyIndex == 2)
        {
            difficulty = "Hard";
        }
    }
}
