using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    [SerializeField] private GameObject spawnListPrefab;

    private GameObject listToSpawnPrefab;
    [SerializeField] private List<GameObject> obstaclesToSpawn;
    private List<GameObject> EnemiesToSpawn;
    private List<GameObject> BossesToSpawn;

    [SerializeField] private List<GameObject> obstaclesPool;
    private List<GameObject> EnemiesPool;
    private List<GameObject> BossesPool;

    private float xSpawnPos = 20.0f;
    private float startDelay = 2f;
    private float obstacleSpawnTime = 1f;
    private float powerupSpawnTime = 5f;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        listToSpawnPrefab = spawnListPrefab.GetComponent<LevelList>().spawnList[GameManager.Instance.levelSelectedNumber];
        obstaclesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().obstacles;
        EnemiesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().enemies;
        BossesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().bosses;

        PoolObstacles();


        InvokeRepeating("SpawnRandomObstacle", startDelay, obstacleSpawnTime);
    }

    void PoolObstacles()
    {
        for(int a = 0; a < 2; a++)
        {
            for (int i = 0; i < obstaclesToSpawn.Count; i++)
            {
                GameObject pooledObstacle = Instantiate(obstaclesToSpawn[i]);
                pooledObstacle.SetActive(false);
                pooledObstacle.AddComponent<Obstacles>();
                obstaclesPool.Add(pooledObstacle);
            }
        }

    }

    private GameObject GetAvailableObstacle()
    {
        //int randomIndex = Random.Range(0, obstaclesPool.Count);
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            if (obstaclesPool[i].activeInHierarchy == false)
                return obstaclesPool[i];
        }         
        return null;
    }

    private void SpawnRandomObstacle()
    {

        int randomPathway = Random.Range(-1, 2);
        GameObject obstacleToSpawn = GetAvailableObstacle();
        //int randomIndex = Random.Range(0, obstaclePrefab.Length);

        Vector3 spawnPos = new Vector3(xSpawnPos, obstacleToSpawn.transform.position.y, randomPathway * playerControllerScript.VerticalStep);

        obstacleToSpawn.SetActive(true);
        obstacleToSpawn.transform.position = spawnPos;
        //Instantiate(obstacleToSpawn, spawnPos, obstacleToSpawn.transform.rotation);

    }
}
