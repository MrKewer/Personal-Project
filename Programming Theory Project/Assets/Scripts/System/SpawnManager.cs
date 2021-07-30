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

    private int PoolDepth = 10; // The amount that will be spawned
    private bool canGrow = true; //If the need for more particals it will create more
    [SerializeField] private int poolDuplicates = 3;

    [Space]
    [Header("Particals Small")]
    [Space]
    [SerializeField] private GameObject yellowSmallParticalPrefab; //Hit Particals
    private List<GameObject> yellowSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject purpleSmallParticalPrefab; //Hit Particals
    private List<GameObject> purpleSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject redSmallParticalPrefab; //Hit Particals
    private List<GameObject> redSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject blueSmallParticalPrefab; //Hit Particals
    private List<GameObject> blueSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject greenSmallParticalPrefab; //Hit Particals
    private List<GameObject> greenSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [Space]
    [Header("Particals Large")]
    [Space]
    [SerializeField] private GameObject yellowLargeParticalPrefab; //Hit Particals
    private List<GameObject> yellowLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject purpleLargeParticalPrefab; //Hit Particals
    private List<GameObject> purpleLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject redLargeParticalPrefab; //Hit Particals
    private List<GameObject> redLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject blueLargeParticalPrefab; //Hit Particals
    private List<GameObject> blueLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject greenLargeParticalPrefab; //Hit Particals
    private List<GameObject> greenLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    //[SerializeField] private GameObject explosionParticalPrefab; //Explosion Particals
    //private List<GameObject> explosionParticalPool = new List<GameObject>();



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
        //Pool Small particals
        PoolGameObject(yellowSmallParticalPrefab, yellowSmallParticalPool);
        PoolGameObject(purpleSmallParticalPrefab, purpleSmallParticalPool);
        PoolGameObject(redSmallParticalPrefab, redSmallParticalPool);
        PoolGameObject(blueSmallParticalPrefab, blueSmallParticalPool);
        PoolGameObject(greenSmallParticalPrefab, greenSmallParticalPool);

        //Pool Large particals
        PoolGameObject(yellowLargeParticalPrefab, yellowLargeParticalPool);
        PoolGameObject(purpleLargeParticalPrefab, purpleLargeParticalPool);
        PoolGameObject(redLargeParticalPrefab, redLargeParticalPool);
        PoolGameObject(blueLargeParticalPrefab, blueLargeParticalPool);
        PoolGameObject(greenLargeParticalPrefab, greenLargeParticalPool);

        //Spawning obstacles with intervals
        InvokeRepeating("SpawnRandomObstacle", startDelay, obstacleSpawnTime);
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.DEAD)
        {
            DisableAllObstacles();
            isPlaying = false;
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.DEAD)
        {
            isPlaying = true;
            DisableAllParticals();
            DisableAllEnemies();
        }
    }

    #region Pool Game Object
    void PoolGameObject(GameObject prefab, List<GameObject> pool) //Create and disable particals used to indicate on hit
    {
        for (int i = 0; i < PoolDepth; i++)
        {
            GameObject pooledParticle = Instantiate(prefab);
            pooledParticle.AddComponent<Obstacles>();
            pooledParticle.SetActive(false);
            pool.Add(pooledParticle);
            pooledParticle.transform.SetParent(gameObject.transform);
        }
    }

    public GameObject GetAvailableGameObject(List<GameObject> pool) //Gets available hit particals
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
                return pool[i];
        }
        return null;
    }
    #endregion

    #region Spawn Partical
    public void SpawnPartical(string partical, Vector3 location)
    {
        GameObject particalEffect;
        switch (partical)
        {
            //Small Particals
            case "YellowSmall":
                particalEffect = GetAvailableGameObject(yellowSmallParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "PurpleSmall":
                particalEffect = GetAvailableGameObject(purpleSmallParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "BlueSmall":
                particalEffect = GetAvailableGameObject(blueSmallParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "RedSmall":
                particalEffect = GetAvailableGameObject(redSmallParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "GreenSmall":
                particalEffect = GetAvailableGameObject(greenSmallParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            //Large Particals
            case "YellowLarge":
                particalEffect = GetAvailableGameObject(yellowLargeParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "PurpleLarge":
                particalEffect = GetAvailableGameObject(purpleLargeParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "BlueLarge":
                particalEffect = GetAvailableGameObject(blueLargeParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "RedLarge":
                particalEffect = GetAvailableGameObject(redLargeParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
            case "GreenLarge":
                particalEffect = GetAvailableGameObject(greenLargeParticalPool);
                if (particalEffect != null)
                {
                    particalEffect.SetActive(true);
                    particalEffect.transform.position = location;
                }
                break;
        }
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
                pooledObstacle.transform.SetParent(gameObject.transform);
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
        //Small Particals
        for (int i = 0; i < yellowSmallParticalPool.Count; i++)
        {
            Destroy(yellowSmallParticalPool[i]);
        }
        for (int i = 0; i < purpleSmallParticalPool.Count; i++)
        {
            Destroy(purpleSmallParticalPool[i]);
        }
        for (int i = 0; i < blueSmallParticalPool.Count; i++)
        {
            Destroy(blueSmallParticalPool[i]);
        }
        for (int i = 0; i < redSmallParticalPool.Count; i++)
        {
            Destroy(redSmallParticalPool[i]);
        }
        for (int i = 0; i < greenSmallParticalPool.Count; i++)
        {
            Destroy(greenSmallParticalPool[i]);
        }

        //Large Particals
        for (int i = 0; i < yellowLargeParticalPool.Count; i++)
        {
            Destroy(yellowLargeParticalPool[i]);
        }
        for (int i = 0; i < purpleLargeParticalPool.Count; i++)
        {
            Destroy(purpleLargeParticalPool[i]);
        }
        for (int i = 0; i < blueLargeParticalPool.Count; i++)
        {
            Destroy(blueLargeParticalPool[i]);
        }
        for (int i = 0; i < redLargeParticalPool.Count; i++)
        {
            Destroy(redLargeParticalPool[i]);
        }
        for (int i = 0; i < greenLargeParticalPool.Count; i++)
        {
            Destroy(greenLargeParticalPool[i]);
        }

        //Other
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

    }

    #region Disable Game Objects

    void DisableAllObstacles()
    {
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            obstaclesPool[i].SetActive(false);
        }
    }
    void DisableAllParticals()
    {
        //Small Particals
        for (int i = 0; i < yellowSmallParticalPool.Count; i++)
        {
            yellowSmallParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < purpleSmallParticalPool.Count; i++)
        {
            purpleSmallParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < redSmallParticalPool.Count; i++)
        {
            redSmallParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < greenSmallParticalPool.Count; i++)
        {
            greenSmallParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < blueSmallParticalPool.Count; i++)
        {
            blueSmallParticalPool[i].SetActive(false);
        }

        //Large Particals
        for (int i = 0; i < yellowLargeParticalPool.Count; i++)
        {
            yellowLargeParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < purpleLargeParticalPool.Count; i++)
        {
            purpleLargeParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < redLargeParticalPool.Count; i++)
        {
            redLargeParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < greenLargeParticalPool.Count; i++)
        {
            greenLargeParticalPool[i].SetActive(false);
        }
        for (int i = 0; i < blueLargeParticalPool.Count; i++)
        {
            blueLargeParticalPool[i].SetActive(false);
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

    #endregion
}
