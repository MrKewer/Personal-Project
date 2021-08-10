using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] //This make it so it can be use to save the list
public class HighScoreList : IComparable<HighScoreList>
{
    public string playerName;
    public int score;
    public string level;
    public string time;

    public HighScoreList(string newPlayerName, int newScore, string newLevel, string newTime)
    {
        playerName = newPlayerName;
        score = newScore;
        level = newLevel;
        time = newTime;
    }
    public int CompareTo(HighScoreList other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return other.score - score;
    }

}
