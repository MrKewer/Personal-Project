using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerControllerScript; //Gets the player Controller Script

    [SerializeField] private GameObject spawnListPrefab; //Gets the list of all obstacles, enemies and bosses

    private GameObject listToSpawnPrefab; //Get the lists of all to spawn based on the selected level (gets from spawnListPrefab's spawnList)
    private List<GameObject> obstaclesToSpawn; //The list that is gotten from the listToSpawnPrefab
    private List<GameObject> enemiesToSpawn;
    private List<GameObject> bossesToSpawn;

    private List<GameObject> obstaclesPool = new List<GameObject>(); //The pool that is created that is used to store the items
    private List<GameObject> enemiesPool = new List<GameObject>();
    private List<GameObject> bossesPool = new List<GameObject>();

    [SerializeField] private List<GameObject> ballsToSpawn;
    private List<GameObject> ballPool = new List<GameObject>();

    private float xObstacleSpawnPos = 30.0f; //The spawn position in the x direction
    private float startDelay = 2f; //Delay before spawning 
    public float obstacleSpawnTime = 0.2f; //Spawning delay intervals
    public float EnemiesSpawnTime = 3f; //Spawning delay intervals
    private float powerupSpawnTime = 5f;
    //private bool isPlaying = true;

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
    [SerializeField] private GameObject graySmallParticalPrefab; //Hit Particals
    private List<GameObject> graySmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items
    [SerializeField] private GameObject whiteSmallParticalPrefab; //Hit Particals
    private List<GameObject> whiteSmallParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

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

    [SerializeField] private GameObject grayLargeParticalPrefab; //Hit Particals
    private List<GameObject> grayLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject whiteLargeParticalPrefab; //Hit Particals
    private List<GameObject> whiteLargeParticalPool = new List<GameObject>(); // The pool that is created that is used to store the items


    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        listToSpawnPrefab = spawnListPrefab.GetComponent<LevelList>().spawnList[GameManager.Instance.levelSelectedNumber];
        obstaclesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().obstacles;
        enemiesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().enemies;
        bossesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().bosses;

        PoolGameObject(obstaclesToSpawn, obstaclesPool, poolDuplicates);
        PoolGameObject(enemiesToSpawn, enemiesPool, poolDuplicates);
        PoolGameObject(bossesToSpawn, bossesPool, 1);

        PoolGameObject(ballsToSpawn, ballPool, poolDuplicates);

        //Pool Small particals
        PoolGameObject(yellowSmallParticalPrefab, yellowSmallParticalPool, PoolDepth);
        PoolGameObject(purpleSmallParticalPrefab, purpleSmallParticalPool, PoolDepth);
        PoolGameObject(redSmallParticalPrefab, redSmallParticalPool, PoolDepth);
        PoolGameObject(blueSmallParticalPrefab, blueSmallParticalPool, PoolDepth);
        PoolGameObject(greenSmallParticalPrefab, greenSmallParticalPool, PoolDepth);
        PoolGameObject(graySmallParticalPrefab, graySmallParticalPool, PoolDepth);
        PoolGameObject(whiteSmallParticalPrefab, whiteSmallParticalPool, PoolDepth);

        //Pool Large particals
        PoolGameObject(yellowLargeParticalPrefab, yellowLargeParticalPool, PoolDepth);
        PoolGameObject(purpleLargeParticalPrefab, purpleLargeParticalPool, PoolDepth);
        PoolGameObject(redLargeParticalPrefab, redLargeParticalPool, PoolDepth);
        PoolGameObject(blueLargeParticalPrefab, blueLargeParticalPool, PoolDepth);
        PoolGameObject(greenLargeParticalPrefab, greenLargeParticalPool, PoolDepth);
        PoolGameObject(grayLargeParticalPrefab, grayLargeParticalPool, PoolDepth);
        PoolGameObject(whiteLargeParticalPrefab, whiteLargeParticalPool, PoolDepth);

        //Spawning obstacles and enemies with intervals
        InvokeRepeating("SpawnObstacle", startDelay, obstacleSpawnTime);
        InvokeRepeating("SpawnEnemy", startDelay, EnemiesSpawnTime);

    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.BOSSFIGHT)
        {
            CancelInvoke("SpawnEnemy");            
            for (int i = 0; i < bossesPool.Count; i++) //Spawn all bosses
            {
                SpawnBoss();
            }
        }
        if (currentState == GameManager.GameState.DEAD)
        {
            CancelInvoke();
            DisableAllObstacles(); //Clean the level, keeps the enemies running
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.DEAD)
        {
            InvokeRepeating("SpawnObstacle", startDelay, obstacleSpawnTime); 
            InvokeRepeating("SpawnEnemy", startDelay, 3);
            DisableAllParticals(); //Clean the level
            DisableAllEnemies(); //Clean the level
        }
    }

    #region Pool Game Object
    void PoolGameObject(GameObject prefab, List<GameObject> pool, int poolSize) //Create and disable particals used to indicate on hit
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pooledGameObject = Instantiate(prefab);
            pooledGameObject.AddComponent<Obstacles>();
            pooledGameObject.SetActive(false);
            pool.Add(pooledGameObject);
            pooledGameObject.transform.SetParent(gameObject.transform);
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

    //Method Overloading
    void PoolGameObject(List<GameObject> prefab, List<GameObject> pool, int duplicates)
    {
        for (int a = 0; a < duplicates; a++) //Create more times to have duplicates in game
        {
            for (int i = 0; i < prefab.Count; i++)
            {
                GameObject pooledGameObject = Instantiate(prefab[i]);
                pooledGameObject.SetActive(false);
                pool.Add(pooledGameObject);
                pooledGameObject.transform.SetParent(gameObject.transform);
            }
        }

    }
    private GameObject GetAvailableRandomGameObject(List<GameObject> pool) //Get an available obstacle
    {
        for (int i = 0; i < 5; i++)
        { //Try to random a few times
            int randomIndex = Random.Range(0, pool.Count);
            if (pool[randomIndex].activeInHierarchy == false)
            {
                return pool[randomIndex];
            }
        }
        return null;
    }
    #endregion

    #region Spawn Partical

    public void SpawnRandomPartical(Vector3 location, float locationRange) // will spawn a random particle at a random location
    {
        int randomNumber = Random.Range(0, (int)Enums.Particals.NumberOfTypes);
        Vector3 randomLocation = new Vector3(Random.Range(-locationRange, locationRange), Random.Range(-locationRange, locationRange), Random.Range(-locationRange, locationRange));
        SpawnParticle((Enums.Particals)randomNumber, randomLocation + location);
    }

    public void SpawnParticle(Enums.Particals partical, Vector3 location)
    {
        GameObject particleEffect;
        switch (partical)
        {
            //Small Particals
            case Enums.Particals.WhiteSmall:
                particleEffect = GetAvailableGameObject(whiteSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.GraySmall:
                particleEffect = GetAvailableGameObject(graySmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.YellowSmall:
                particleEffect = GetAvailableGameObject(yellowSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.PurpleSmall:
                particleEffect = GetAvailableGameObject(purpleSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.BlueSmall:
                particleEffect = GetAvailableGameObject(blueSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.RedSmall:
                particleEffect = GetAvailableGameObject(redSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.GreenSmall:
                particleEffect = GetAvailableGameObject(greenSmallParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            //Large Particals
            case Enums.Particals.WhiteLarge:
                particleEffect = GetAvailableGameObject(whiteLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.GrayLarge:
                particleEffect = GetAvailableGameObject(grayLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;

            case Enums.Particals.YellowLarge:
                particleEffect = GetAvailableGameObject(yellowLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.PurpleLarge:
                particleEffect = GetAvailableGameObject(purpleLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.BlueLarge:
                particleEffect = GetAvailableGameObject(blueLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.RedLarge:
                particleEffect = GetAvailableGameObject(redLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particals.GreenLarge:
                particleEffect = GetAvailableGameObject(greenLargeParticalPool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
        }
    }
    #endregion

    #region Obstacles

    private void SpawnObstacle() //Will set an random obstacle active and set it to a new position
    {
        int randomPathway = Random.Range(-1, 2);
        GameObject obstacleToSpawn = GetAvailableRandomGameObject(obstaclesPool);

        if (obstacleToSpawn != null)
        {
            float yPos = obstacleToSpawn.transform.position.y;
            Vector3 spawnPos = new Vector3(xObstacleSpawnPos, yPos, randomPathway * playerControllerScript.VerticalStep);

            obstacleToSpawn.SetActive(true);
            obstacleToSpawn.transform.position = spawnPos;
        }
    }

    public void SpawnBall(Vector3 location) //Will set an random obstacle active and set it to a new position
    {
        GameObject ballToSpawn = GetAvailableRandomGameObject(ballPool);

        if (ballToSpawn != null)
        {
            ballToSpawn.SetActive(true);
            //ballToSpawn.GetComponent<Rigidbody>().AddForce(Vector3.left * 100, ForceMode.Impulse);
            ballToSpawn.transform.position = location;
        }
    }

    #endregion

    #region Enemies

    private void SpawnEnemy() //Will set an random obstacle active and set it to a new position
    {
        if (GameManager.Instance.basicEnemiesCount <= GameManager.Instance.maxSpawnedEnemies)
        {
            int randomPathway = Random.Range(-1, 2);
            GameObject enemyToSpawn = GetAvailableRandomGameObject(enemiesPool);

            if (enemyToSpawn != null)
            {
                EnemyMain enemyScript = enemyToSpawn.GetComponent<EnemyMain>();
                float xPos = Random.Range(enemyScript.spawnPos - enemyScript.spawnPosRange, enemyScript.spawnPos + enemyScript.spawnPosRange);
                float yPos = enemyToSpawn.transform.position.y;
                Vector3 spawnPos = new Vector3(xPos, yPos, randomPathway * playerControllerScript.VerticalStep);

                enemyToSpawn.SetActive(true);
                enemyToSpawn.transform.position = spawnPos;
            }
        }
    }
    #endregion

    #region Bosses

    private void SpawnBoss() //Will set an random obstacle active and set it to a new position
    {
        int randomPathway = Random.Range(-1, 2);
        GameObject bossToSpawn = GetAvailableRandomGameObject(bossesPool);

        if (bossToSpawn != null)
        {
            EnemyMain enemyScript = bossToSpawn.GetComponent<EnemyMain>();
            float xPos = Random.Range(enemyScript.spawnPos - enemyScript.spawnPosRange, enemyScript.spawnPos + enemyScript.spawnPosRange);
            float yPos = bossToSpawn.transform.position.y;
            Vector3 spawnPos = new Vector3(xPos, yPos, randomPathway * playerControllerScript.VerticalStep);

            bossToSpawn.SetActive(true);
            bossToSpawn.transform.position = spawnPos;
        }
    }

    public void BossDeath(Vector3 location) //Will clear level + spawn a few particles
    {
        CancelInvoke();
        DisableAllObstacles();
        DisableAllEnemies();
        StartCoroutine(CoSpawnParticles(location));
    }
    IEnumerator CoSpawnParticles(Vector3 location)
    {
        int count = 0;
        while (count <= 10)
        {
            float randomTime = Random.Range(0, 0.75f);
            WaitForSeconds waitTime = new WaitForSeconds(randomTime);
            yield return waitTime;
            SpawnRandomPartical(location, 10);
            count++;
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
        for (int i = 0; i < enemiesPool.Count; i++)
        {
            Destroy(enemiesPool[i]);
        }
        for (int i = 0; i < bossesPool.Count; i++)
        {
            Destroy(bossesPool[i]);
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
        for (int i = 0; i < enemiesPool.Count; i++)
        {
            enemiesPool[i].SetActive(false);
        }
        for (int i = 0; i < bossesPool.Count; i++)
        {
            bossesPool[i].SetActive(false);
        }
    }

    #endregion
}
