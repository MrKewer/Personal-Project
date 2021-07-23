using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    [SerializeField] private GameObject spawnListPrefab;

    private GameObject listToSpawnPrefab;
    private List<GameObject> obstaclesToSpawn;
    private List<GameObject> EnemiesToSpawn;
    private List<GameObject> BossesToSpawn;

    private List<GameObject> obstaclesPool = new List<GameObject>();
    private List<GameObject> EnemiesPool;
    private List<GameObject> BossesPool;

    private float xSpawnPos = 30.0f;
    private float startDelay = 2f;
    public float obstacleSpawnTime = 0.5f;
    private float powerupSpawnTime = 5f;


    [SerializeField] private GameObject obstacleHitParticalPrefab;
    [SerializeField] private GameObject explosionParticalPrefab;
    private int particalPoolDepth = 10;
    private bool canGrow = true;
    private List<GameObject> obstacleHitParticalPool = new List<GameObject>();
    private List<GameObject> explosionParticalPool = new List<GameObject>();

    [SerializeField] private int poolDuplicates = 3;

    private void Awake()
    {



    }
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        listToSpawnPrefab = spawnListPrefab.GetComponent<LevelList>().spawnList[GameManager.Instance.levelSelectedNumber];
        obstaclesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().obstacles;
        EnemiesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().enemies;
        BossesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().bosses;
        PoolObstacles();
        PoolObsticalHitPartical();

        InvokeRepeating("SpawnRandomObstacle", startDelay, obstacleSpawnTime);
    }
    #region Obstacles

    void PoolObsticalHitPartical()
    {
        for(int i=0; i<particalPoolDepth; i++)
        {
            GameObject pooledParticle = Instantiate(obstacleHitParticalPrefab);
            pooledParticle.AddComponent<Obstacles>();
            pooledParticle.SetActive(false);
            obstacleHitParticalPool.Add(pooledParticle);
        }
    }

    public GameObject GetAvailableObstacleHitPartical()
    {
        for(int i = 0; i < obstacleHitParticalPool.Count; i++)
        {
            if (obstacleHitParticalPool[i].activeInHierarchy == false)
                return obstacleHitParticalPool[i];
        }
        if (canGrow == true)
        {
            GameObject pooledParticle = Instantiate(obstacleHitParticalPrefab);
            pooledParticle.AddComponent<Obstacles>();
            pooledParticle.SetActive(false);
            obstacleHitParticalPool.Add(pooledParticle);
            return pooledParticle;
        }
        else
            return null;
    }

    void PoolObstacles()
    {
        for(int a = 0; a < 3; a++)
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
        for (int i = 0; i < 5; i++) { //Try to random a few times
            int randomIndex = Random.Range(0, obstaclesPool.Count);
            if (obstaclesPool[randomIndex].activeInHierarchy == false)
            {
                return obstaclesPool[randomIndex];
            }
        }
        return null;
    }

    private void SpawnRandomObstacle()
    {
        int randomPathway = Random.Range(-1, 2);
        GameObject obstacleToSpawn = GetAvailableObstacle();   

        if (obstacleToSpawn != null)
        {
            float yPos = obstacleToSpawn.transform.position.y;
            Vector3 spawnPos = new Vector3(xSpawnPos, yPos, randomPathway * playerControllerScript.VerticalStep);

            obstacleToSpawn.SetActive(true);
            obstacleToSpawn.transform.position = spawnPos;
        }
    }
    #endregion
}
