using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    public List<Sprite> levelSelectList; //The list that is displayed in the main menu to select the level
    public List<Sprite> backgroundList; //The list of all the levels' background 
    public List<Sprite> groundList; //The list of all the levels' ground
    public List<GameObject> spawnList; //The lists of all the obsticals, enemies to spawn depending which level is selected
}
