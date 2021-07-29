using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField] private GameObject playerPrefab;
    private PlayerController playerControllerScript; //Gets the player Controller Script
    //private GameObject characterSelected;
    //[SerializeField] private GameObject CharacterListPrefab;

    [SerializeField] private GameObject spawnListPrefab; //Gets the list of all obstacles, enemies and bosses

    private GameObject listToSpawnPrefab; //Get the lists of all to spawn based on the selected level (gets from spawnListPrefab's spawnList)
    private List<GameObject> obstaclesToSpawn; //The list that is gotten from the listToSpawnPrefab
    private List<GameObject> EnemiesToSpawn;
    private List<GameObject> BossesToSpawn;

    private List<GameObject> obstaclesPool = new List<GameObject>(); //The pool that is created that is used to store the items
    private List<GameObject> EnemiesPool = new List<GameObject>();
    private List<GameObject> BossesPool = new List<GameObject>();

    private float xSpawnPos = 30.0f; //The spawn position in the x direction
    private float startDelay = 2f; //Delay before spawning 
    public float obstacleSpawnTime = 0.2f; //Spawning delay intervals
    private float powerupSpawnTime = 5f;
    private bool isPlaying = true;

    [SerializeField] private GameObject obstacleHitParticalPrefab; //Hit Particals
    [SerializeField] private GameObject explosionParticalPrefab; //Explosion Particals
    private int particalPoolDepth = 10; // The amount that will be spawned
    private bool canGrow = true; //If the need for more particals it will create more
    private List<GameObject> obstacleHitParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items
    private List<GameObject> explosionParticalPool = new List<GameObject>();

    [SerializeField] private int poolDuplicates = 3;

    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        listToSpawnPrefab = spawnListPrefab.GetComponent<LevelList>().spawnList[GameManager.Instance.levelSelectedNumber];
        obstaclesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().obstacles;
        EnemiesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().enemies;
        BossesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().bosses;

        //Pool the needed objects
        PoolObstacles();
        PoolObsticalHitPartical();

        //Spawning obstacles with intervals
        InvokeRepeating("SpawnRandomObstacle", startDelay, obstacleSpawnTime);
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.DEAD)
        {
            DisableAllObstacles();
            DisableAllEnemies();
            isPlaying = false;
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.DEAD)
        {
            isPlaying = true;
            DisableAllParticals();
        }
    }


    #region Hit Partical

    void PoolObsticalHitPartical() //Create and disable particals used to indicate on hit
    {
        for (int i = 0; i < particalPoolDepth; i++)
        {
            GameObject pooledParticle = Instantiate(obstacleHitParticalPrefab);
            pooledParticle.AddComponent<Obstacles>();
            pooledParticle.SetActive(false);
            obstacleHitParticalPool.Add(pooledParticle);
        }
    }

    public GameObject GetAvailableObstacleHitPartical() //Gets available hit particals
    {
        for (int i = 0; i < obstacleHitParticalPool.Count; i++)
        {
            if (obstacleHitParticalPool[i].activeInHierarchy == false)
                return obstacleHitParticalPool[i];
        }
        if (canGrow == true) //If there arent enough, it will create more
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
    #endregion

    #region Obstacles
    void PoolObstacles() //Create and disable obstacles
    {
        for (int a = 0; a < poolDuplicates; a++) //Create more times to have duplicates in game
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

    private GameObject GetAvailableObstacle() //Get an available obstacle
    {
        for (int i = 0; i < 5; i++)
        { //Try to random a few times
            int randomIndex = Random.Range(0, obstaclesPool.Count);
            if (obstaclesPool[randomIndex].activeInHierarchy == false)
            {
                return obstaclesPool[randomIndex];
            }
        }
        return null;
    }

    private void SpawnRandomObstacle() //Will set an random obstacle active and set it to a new position
    {
        if (isPlaying)
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
    }
    #endregion


    private void OnDestroy() //Clear all created objects from game
    {
        for (int i = 0; i < obstacleHitParticalPool.Count; i++)
        {
            Destroy(obstacleHitParticalPool[i]);
        }
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            Destroy(obstaclesPool[i]);
        }
        for (int i = 0; i < EnemiesPool.Count; i++)
        {
            Destroy(EnemiesPool[i]);
        }
        for (int i = 0; i < BossesPool.Count; i++)
        {
            Destroy(BossesPool[i]);
        }
        for (int i = 0; i < explosionParticalPool.Count; i++)
        {
            Destroy(explosionParticalPool[i]);
        }
    }

    void DisableAllObstacles()
    {
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            obstaclesPool[i].SetActive(false);
        }
    }
    void DisableAllParticals()
    {
        for (int i = 0; i < obstacleHitParticalPool.Count; i++)
        {
            obstacleHitParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < explosionParticalPool.Count; i++)
        {
            explosionParticalPool[i].SetActive(false);
        }
    }
    void DisableAllEnemies()
    {
        for (int i = 0; i < EnemiesPool.Count; i++)
        {
            EnemiesPool[i].SetActive(false);
        }
        for (int i = 0; i < BossesPool.Count; i++)
        {
            BossesPool[i].SetActive(false);
        }

    }

}
