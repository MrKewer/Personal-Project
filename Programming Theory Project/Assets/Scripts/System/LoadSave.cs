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
        for (int i = 0; i < 6; i++)
        {
            highScores.Add(new HighScoreList("", 0, "", ""));
        }
        if (PlayerPrefs.GetString("HighScores") != null)
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("HighScores"), this);
        }
        GameManager.Instance.highScores = highScores;
    }
    public void SaveGame()
    {
        highScores = GameManager.Instance.highScores;
        PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(this));
        PlayerPrefs.Save();
    }
}
