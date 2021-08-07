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




    private float xObstacleSpawnPos = 30.0f; //The spawn position in the x direction
    private float startDelay = 2f; //Delay before spawning 
    public float obstacleSpawnTime = 0.2f; //Spawning delay intervals
    public float EnemiesSpawnTime = 3f; //Spawning delay intervals
    public float pickupSpawnTime = 0.2f;
    //private bool isPlaying = true;

    private int PoolDepth = 10; // The amount that will be spawned
    //private bool canGrow = true; //If the need for more particles it will create more
    [SerializeField] private int poolDuplicates = 3;

    [Space]
    [Header("Pickups")]
    [Space]
    [SerializeField] private List<GameObject> pickupsToSpawn; //All the pickups to spawn
    private List<GameObject> pickupPool = new List<GameObject>(); //The pool that is created that is used to store the items
    [SerializeField] private List<GameObject> ballsToSpawn; //The list of the ball to spawn
    private List<GameObject> ballPool = new List<GameObject>(); //The pool that is created that is used to store the items
    [SerializeField] private GameObject bombPrefab; //The Bomb that is droped
    private List<GameObject> bombPool = new List<GameObject>(); //The pool that is created that is used to store the items
    [SerializeField] private GameObject ExplosionParticlePrefab; //The explosion the bomb will create
    private List<GameObject> ExplosionParticlePool = new List<GameObject>(); //The pool that is created that is used to store the items
    private int PickupCounter = 0; //Keeps count of obsticles spawned
    private int WhenToSpawnPickup = 4; //Spawn pickup after this amount of obsticles has spawned, will be randomed.

    #region Prefabs for particles
    [Space]
    [Header("Particles Small")]
    [Space]
    [SerializeField] private GameObject yellowSmallParticlePrefab; //Hit Particles
    private List<GameObject> yellowSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject purpleSmallParticlePrefab; //Hit Particles
    private List<GameObject> purpleSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject redSmallParticlePrefab; //Hit Particles
    private List<GameObject> redSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject blueSmallParticlePrefab; //Hit Particles
    private List<GameObject> blueSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject greenSmallParticlePrefab; //Hit Particles
    private List<GameObject> greenSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items
    [SerializeField] private GameObject graySmallParticlePrefab; //Hit Particles
    private List<GameObject> graySmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items
    [SerializeField] private GameObject whiteSmallParticlePrefab; //Hit Particles
    private List<GameObject> whiteSmallParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [Space]
    [Header("Particles Large")]
    [Space]
    [SerializeField] private GameObject yellowLargeParticlePrefab; //Hit Particles
    private List<GameObject> yellowLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject purpleLargeParticlePrefab; //Hit Particles
    private List<GameObject> purpleLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject redLargeParticlePrefab; //Hit Particles
    private List<GameObject> redLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject blueLargeParticlePrefab; //Hit Particles
    private List<GameObject> blueLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject greenLargeParticlePrefab; //Hit Particles
    private List<GameObject> greenLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject grayLargeParticlePrefab; //Hit Particles
    private List<GameObject> grayLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    [SerializeField] private GameObject whiteLargeParticlePrefab; //Hit Particles
    private List<GameObject> whiteLargeParticlePool = new List<GameObject>(); // The pool that is created that is used to store the items

    #endregion

    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event

        
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        //Pool Obsticles + Enemies + Bosses
        listToSpawnPrefab = spawnListPrefab.GetComponent<LevelList>().spawnList[GameManager.Instance.levelSelectedNumber];
        obstaclesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().obstacles;
        enemiesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().enemies;
        bossesToSpawn = listToSpawnPrefab.GetComponent<SpawnList>().bosses;

        PoolGameObject(obstaclesToSpawn, obstaclesPool, poolDuplicates);
        PoolGameObject(enemiesToSpawn, enemiesPool, poolDuplicates);
        PoolGameObject(bossesToSpawn, bossesPool, 1);

        //Pool Pickups
        PoolGameObject(pickupsToSpawn, pickupPool, poolDuplicates);
        PoolGameObject(ballsToSpawn, ballPool, poolDuplicates);
        PoolGameObject(bombPrefab, bombPool, 2);
        PoolGameObject(ExplosionParticlePrefab, ExplosionParticlePool, 2);

        //Pool Small particles
        PoolGameObject(yellowSmallParticlePrefab, yellowSmallParticlePool, PoolDepth);
        PoolGameObject(purpleSmallParticlePrefab, purpleSmallParticlePool, PoolDepth);
        PoolGameObject(redSmallParticlePrefab, redSmallParticlePool, PoolDepth);
        PoolGameObject(blueSmallParticlePrefab, blueSmallParticlePool, PoolDepth);
        PoolGameObject(greenSmallParticlePrefab, greenSmallParticlePool, PoolDepth);
        PoolGameObject(graySmallParticlePrefab, graySmallParticlePool, PoolDepth);
        PoolGameObject(whiteSmallParticlePrefab, whiteSmallParticlePool, PoolDepth);

        //Pool Large particles
        PoolGameObject(yellowLargeParticlePrefab, yellowLargeParticlePool, PoolDepth);
        PoolGameObject(purpleLargeParticlePrefab, purpleLargeParticlePool, PoolDepth);
        PoolGameObject(redLargeParticlePrefab, redLargeParticlePool, PoolDepth);
        PoolGameObject(blueLargeParticlePrefab, blueLargeParticlePool, PoolDepth);
        PoolGameObject(greenLargeParticlePrefab, greenLargeParticlePool, PoolDepth);
        PoolGameObject(grayLargeParticlePrefab, grayLargeParticlePool, PoolDepth);
        PoolGameObject(whiteLargeParticlePrefab, whiteLargeParticlePool, PoolDepth);



        //Spawning obstacles and enemies with intervals
        InvokeRepeating("SpawnObstacleAndPickup", startDelay, obstacleSpawnTime);
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
            DisableAllParticles(); //Clean the level
            DisableAllEnemies(); //Clean the level
        }
    }

    #region Pool Game Object
    void PoolGameObject(GameObject prefab, List<GameObject> pool, int poolSize) //Create and disable GameObjects
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pooledGameObject = Instantiate(prefab);
            pooledGameObject.AddComponent<MoveLeft>();
            pooledGameObject.SetActive(false);
            pool.Add(pooledGameObject);
            pooledGameObject.transform.SetParent(gameObject.transform);
        }
    }

    public GameObject GetAvailableGameObject(List<GameObject> pool) //Gets available hit particles
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

    #region Spawn Particle

    public void SpawnRandomParticle(Vector3 location, float locationRange) // will spawn a random particle at a random location
    {
        int randomNumber = Random.Range(0, (int)Enums.Particles.NumberOfTypes);
        Vector3 randomLocation = new Vector3(Random.Range(-locationRange, locationRange), Random.Range(-locationRange, locationRange), Random.Range(-locationRange, locationRange));
        SpawnParticle((Enums.Particles)randomNumber, randomLocation + location);
    }

    public void SpawnParticle(Enums.Particles particle, Vector3 location)
    {
        GameObject particleEffect;
        switch (particle)
        {
            //Small Particles
            case Enums.Particles.WhiteSmall:
                particleEffect = GetAvailableGameObject(whiteSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.GraySmall:
                particleEffect = GetAvailableGameObject(graySmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.YellowSmall:
                particleEffect = GetAvailableGameObject(yellowSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.PurpleSmall:
                particleEffect = GetAvailableGameObject(purpleSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.BlueSmall:
                particleEffect = GetAvailableGameObject(blueSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.RedSmall:
                particleEffect = GetAvailableGameObject(redSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.GreenSmall:
                particleEffect = GetAvailableGameObject(greenSmallParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            //Large Particles
            case Enums.Particles.WhiteLarge:
                particleEffect = GetAvailableGameObject(whiteLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.GrayLarge:
                particleEffect = GetAvailableGameObject(grayLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;

            case Enums.Particles.YellowLarge:
                particleEffect = GetAvailableGameObject(yellowLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.PurpleLarge:
                particleEffect = GetAvailableGameObject(purpleLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.BlueLarge:
                particleEffect = GetAvailableGameObject(blueLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.RedLarge:
                particleEffect = GetAvailableGameObject(redLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
            case Enums.Particles.GreenLarge:
                particleEffect = GetAvailableGameObject(greenLargeParticlePool);
                if (particleEffect != null)
                {
                    particleEffect.SetActive(true);
                    particleEffect.transform.position = location;
                }
                break;
        }
    }
    #endregion

    #region Obstacles and Pickups

    private void SpawnObstacleAndPickup() //Will set an random obstacle and pickup active and set it to a new position
    {
        //spawn Obstacles
        int randomPathwayObstacle = Random.Range(-1, 2);
        GameObject obstacleToSpawn = GetAvailableRandomGameObject(obstaclesPool);
        if (obstacleToSpawn != null)
        {
            float yPos = obstacleToSpawn.transform.position.y;
            Vector3 spawnPos = new Vector3(xObstacleSpawnPos, yPos, randomPathwayObstacle * playerControllerScript.VerticalStep);

            obstacleToSpawn.SetActive(true);
            obstacleToSpawn.transform.position = spawnPos;
        }

        //Spawn Pickup
        if (PickupCounter >= pickupSpawnTime)
        {
            int randomPathwayPickup = RandomDifferentPathway(randomPathwayObstacle);
            if (randomPathwayPickup != -5) //-5 means invalid
            {
                GameObject pickupToSpawn = GetAvailableRandomGameObject(pickupPool);
                if (pickupToSpawn != null)
                {
                    float yPos = pickupToSpawn.transform.position.y;
                    Vector3 spawnPos = new Vector3(xObstacleSpawnPos, yPos, randomPathwayPickup * playerControllerScript.VerticalStep);

                    pickupToSpawn.SetActive(true);
                    pickupToSpawn.transform.position = spawnPos;
                }
                pickupSpawnTime = Random.Range(2, 7);
                PickupCounter = 0;
            }
        }
        PickupCounter++;
    }
    private int RandomDifferentPathway(int obstaclePathway)
    {
        for (int i = 0; i < 5; i++)
        {
            int randomPathwayPickup = Random.Range(-1, 2);
            if (randomPathwayPickup != obstaclePathway)
            {
                return randomPathwayPickup;
            }
        }
        return -5; //set an invalid number
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
            SpawnRandomParticle(location, 10);
            count++;
        }

    }

    #endregion

    #region Explosion
    public void SpawnBomb(Vector3 spawnPos)
    {
        GameObject bombToSpawn = GetAvailableRandomGameObject(bombPool);

        if (bombToSpawn != null)
        {
            bombToSpawn.SetActive(true);
            bombToSpawn.transform.position = spawnPos;
        }

    }

    public void SpawnExplosion(Vector3 spawnPos)
    {
        GameObject explosionToSpawn = GetAvailableRandomGameObject(ExplosionParticlePool);

        if (explosionToSpawn != null)
        {
            explosionToSpawn.SetActive(true);
            explosionToSpawn.transform.position = spawnPos;
        }
    }


    #endregion
    private void OnDestroy() //Clear all created objects from game
    {
        //Small Particles
        for (int i = 0; i < yellowSmallParticlePool.Count; i++)
        {
            Destroy(yellowSmallParticlePool[i]);
        }
        for (int i = 0; i < purpleSmallParticlePool.Count; i++)
        {
            Destroy(purpleSmallParticlePool[i]);
        }
        for (int i = 0; i < blueSmallParticlePool.Count; i++)
        {
            Destroy(blueSmallParticlePool[i]);
        }
        for (int i = 0; i < redSmallParticlePool.Count; i++)
        {
            Destroy(redSmallParticlePool[i]);
        }
        for (int i = 0; i < greenSmallParticlePool.Count; i++)
        {
            Destroy(greenSmallParticlePool[i]);
        }

        //Large Particles
        for (int i = 0; i < yellowLargeParticlePool.Count; i++)
        {
            Destroy(yellowLargeParticlePool[i]);
        }
        for (int i = 0; i < purpleLargeParticlePool.Count; i++)
        {
            Destroy(purpleLargeParticlePool[i]);
        }
        for (int i = 0; i < blueLargeParticlePool.Count; i++)
        {
            Destroy(blueLargeParticlePool[i]);
        }
        for (int i = 0; i < redLargeParticlePool.Count; i++)
        {
            Destroy(redLargeParticlePool[i]);
        }
        for (int i = 0; i < greenLargeParticlePool.Count; i++)
        {
            Destroy(greenLargeParticlePool[i]);
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
        for (int i = 0; i < pickupPool.Count; i++)
        {
            Destroy(pickupPool[i]);
        }
        for (int i = 0; i < ballPool.Count; i++)
        {
            Destroy(ballPool[i]);
        }
        for (int i = 0; i < bombPool.Count; i++)
        {
            Destroy(bombPool[i]);
        }
        for (int i = 0; i < ExplosionParticlePool.Count; i++)
        {
            Destroy(ExplosionParticlePool[i]);
        }
        
    }

    #region Disable Game Objects

    void DisableAllObstacles()
    {
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            obstaclesPool[i].SetActive(false);
        }
        for (int i = 0; i < pickupPool.Count; i++)
        {
            Destroy(pickupPool[i]);
        }
        for (int i = 0; i < ballPool.Count; i++)
        {
            Destroy(ballPool[i]);
        }
        for (int i = 0; i < bombPool.Count; i++)
        {
            Destroy(bombPool[i]);
        }
    }
    void DisableAllParticles()
    {
        //Small Particles
        for (int i = 0; i < yellowSmallParticlePool.Count; i++)
        {
            yellowSmallParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < purpleSmallParticlePool.Count; i++)
        {
            purpleSmallParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < redSmallParticlePool.Count; i++)
        {
            redSmallParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < greenSmallParticlePool.Count; i++)
        {
            greenSmallParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < blueSmallParticlePool.Count; i++)
        {
            blueSmallParticlePool[i].SetActive(false);
        }

        //Large Particles
        for (int i = 0; i < yellowLargeParticlePool.Count; i++)
        {
            yellowLargeParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < purpleLargeParticlePool.Count; i++)
        {
            purpleLargeParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < redLargeParticlePool.Count; i++)
        {
            redLargeParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < greenLargeParticlePool.Count; i++)
        {
            greenLargeParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < blueLargeParticlePool.Count; i++)
        {
            blueLargeParticlePool[i].SetActive(false);
        }
        for (int i = 0; i < ExplosionParticlePool.Count; i++)
        {
            Destroy(ExplosionParticlePool[i]);
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
