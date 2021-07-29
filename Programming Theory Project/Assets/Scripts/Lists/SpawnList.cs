using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Each Level has its own SpawnList
public class SpawnList : MonoBehaviour
{
    public List<GameObject> obstacles; //All the obstacles to spawn for the Level
    public List<GameObject> enemies; //All the enemies to spawn for the level
    public List<GameObject> bosses; //All the bosses to spawn for the level
}
