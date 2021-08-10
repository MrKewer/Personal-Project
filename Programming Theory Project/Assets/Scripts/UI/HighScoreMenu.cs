using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreMenu : MonoBehaviour
{
    private List<HighScoreList> highScores;

    [SerializeField] private TextMeshProUGUI firstPlayerName;
    [SerializeField] private TextMeshProUGUI firstPlayerLevel;

    [SerializeField] private TextMeshProUGUI secondPlayerName;
    [SerializeField] private TextMeshProUGUI secondPlayerLevel;

    [SerializeField] private TextMeshProUGUI thirdPlayerName;
    [SerializeField] private TextMeshProUGUI thirdPlayerLevel;

    [SerializeField] private TextMeshProUGUI fourthPlayerName;
    [SerializeField] private TextMeshProUGUI fourthPlayerLevel;

    [SerializeField] private TextMeshProUGUI fifthPlayerName;
    [SerializeField] private TextMeshProUGUI fifthPlayerLevel;

    private void OnEnable()
    {
        highScores = GameManager.Instance.highScores; //Gets the list from GameManager

        if (highScores[0].score > 0) //Check to see if there are data otherwise show empty text fields
        {
            highScores.Sort();
            firstPlayerName.text = highScores[0].playerName + " : " + highScores[0].score;
            firstPlayerLevel.text = highScores[0].level + " : " + highScores[0].time;
        }
        else
        {
            firstPlayerName.text = "None... Play to";
            firstPlayerLevel.text = "make new ones";
        }


        if (highScores[1].score > 0)
        {
            secondPlayerName.text = highScores[1].playerName + " : " + highScores[1].score;
            secondPlayerLevel.text = highScores[1].level + " : " + highScores[1].time;
        }
        else
        {
            secondPlayerName.text = "";
            secondPlayerLevel.text = "";
        }

        if (highScores[2].score > 0)
        {
            thirdPlayerName.text = highScores[2].playerName + " : " + highScores[2].score;
            thirdPlayerLevel.text = highScores[2].level + " : " + highScores[2].time;
        }
        else
        {
            thirdPlayerName.text = "";
            thirdPlayerLevel.text = "";
        }

        if (highScores[3].score > 0)
        {
            fourthPlayerName.text = highScores[3].playerName + " : " + highScores[3].score;
            fourthPlayerLevel.text = highScores[3].level + " : " + highScores[3].time;
        }
        else
        {
            fourthPlayerName.text = "";
            fourthPlayerLevel.text = "";
        }

        if (highScores[4].score > 0)
        {
            fifthPlayerName.text = highScores[4].playerName + " : " + highScores[4].score;
            fifthPlayerLevel.text = highScores[4].level + " : " + highScores[4].time;
        }
        else
        {
            fifthPlayerName.text = "";
            fifthPlayerLevel.text = "";
        }
    }
}
