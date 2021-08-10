using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //Must be included to make the class serializable

[Serializable] //This make it so it can be use to save lists
public class LoadSave : MonoBehaviour
{
    public List<HighScoreList> highScores = new List<HighScoreList>();

    public void LoadGame()
    {
        for (int i = 0; i < 6; i++) //Needs to create 6 items in the list, to prevent errors
        {
            highScores.Add(new HighScoreList("", 0, "", ""));
        }
        if (PlayerPrefs.GetString("HighScores") != null) //if the load file exist, otherwise it will clear the list
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("HighScores"), this);
        }
        GameManager.Instance.highScores = highScores; //Set the list created/loaded equal to GameManager's list
    }
    public void SaveGame()
    {
        highScores = GameManager.Instance.highScores; //Gets GameManager's list
        PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(this)); //Convert to Json format
        PlayerPrefs.Save(); //Saves the list
    }
}
