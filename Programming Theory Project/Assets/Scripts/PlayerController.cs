using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void DamageDealtHandler(float amount); //Create a delegate to use when damage is dealt
public class PlayerController : MonoBehaviour, IDamageable<float, Enums.DamageType, Vector3>
{
    public event DamageDealtHandler DamageDealt; //This event will be used to update the InGameUI
    private GameObject characterSelected; //The gameObject to spawn the character selected in MainMenu
    [SerializeField] private GameObject CharacterListPrefab; //The List of the Characters 
    private Animator runAnimation; //Gets the run animation to control the speed of the animation if the background speed increase
    [SerializeField] private float speed = 10.0f; //Forward speed when user press forward and backwards
    [SerializeField] private float verticalSpeed = 10.0f; //The speed used basically when changing lanes
    [SerializeField] private float verticalStep = 3f; //The distance of the lanes
    [SerializeField] private float xBound = 10.0f; // the maximum position the player can go forward or backwards
    [SerializeField] private float jumpForce = 250.0f;
    private bool bIsDead = false;
    [SerializeField] private bool isOnGround; //To be able to jump or not
    [SerializeField] private bool moveLeft = false; //Move from one lane to the other lane
    [SerializeField] private bool moveCenterFL = false; //Move from one lane to the other lane
    [SerializeField] private bool moveCenterFR = false; //Move from one lane to the other lane
    [SerializeField] private bool moveRight = false; //Move from one lane to the other lane

    public Enums.Pickups currentPowerup;
    [SerializeField] private int coinValue = 10; //The value of each coin picked up
    public bool bHasDoubleCoins = false; //Used to make the player receive double coins
    [SerializeField] private bool invulnerable = false; //Make the player invulnerable to damage
    [SerializeField] private float healPickup = 50f; //Full health

    public float maxHealth = 100f; //Full health
    public float health = 100f; //Current health
    public int score = 0; //The Main score is kept here
    private Rigidbody playerRb;
    private Vector3 startPos; //The start position, used to calculate the different lanes
    public SpawnManager spawnManager; //Gets reference of the SpawnManager
    public InGameUI inGameUI; //This is set inside of the InGameUI

    public float VerticalStep // The size of the lanes
    {
        get { return verticalStep; }
        private set { }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event

        //Spawn the selected character
        Vector3 characterTransform = new Vector3(0, 0, 0); 
        Quaternion characterRotation = Quaternion.Euler(0, 90, 0);
        characterSelected = CharacterListPrefab.GetComponent<CharacterList>().characterList[GameManager.Instance.characterSelectedNumber];
        characterSelected = Instantiate(characterSelected, characterTransform, characterRotation);
        characterSelected.transform.SetParent(gameObject.transform);

        playerRb = GetComponent<Rigidbody>();
        startPos = transform.position; //Used to be able to calculate the different lanes
        runAnimation = characterSelected.GetComponent<Animator>();
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.RUNNING && (previousState == GameManager.GameState.DEAD || previousState == GameManager.GameState.ENDGAME))
        {
            resetAll();
        }
    }
    public void resetAll() //When game restarts 
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        moveLeft = false;
        moveCenterFL = false;
        moveCenterFR = false;
        moveRight = false;
        health = maxHealth;
        score = 0;
        characterSelected.SetActive(true);
        bIsDead = false;
        inGameUI.UpdatePlayerHealth();
        inGameUI.UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayerPosition();

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround) //Jump
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    #region Player Movement
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 leftPos = new Vector3(transform.position.x, transform.position.y, verticalStep);
        Vector3 centerPos = new Vector3(transform.position.x, transform.position.y, startPos.z);
        Vector3 rightPos = new Vector3(transform.position.x, transform.position.y, -verticalStep);

        // move player forward and backwards
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
        runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed / 10);
        // move player from center to left
        #region Move Left
        // when the player is in position of the center
        if (verticalInput > 0 && transform.position.z >= startPos.z && transform.position.z < verticalStep)
        {
            moveLeft = true;
            moveCenterFL = false;
            moveCenterFR = false;
            moveRight = false;
        }
        // only need a impulse input
        if (moveLeft)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, Time.deltaTime * verticalSpeed);
        }
        // When the player is in position 
        if (transform.position.z >= verticalStep)
        {
            moveLeft = false;
        }
        #endregion

        // move player from left to center
        #region Move Center From Left 
        // when the player is in position of the Left side
        if (verticalInput < 0 && transform.position.z > startPos.z && transform.position.z <= verticalStep)
        {
            moveLeft = false;
            moveCenterFL = true;
            moveCenterFR = false;
            moveRight = false;
        }
        // only need a impulse input
        if (moveCenterFL)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPos, Time.deltaTime * verticalSpeed);
        }
        // When the player is in position 
        if (transform.position.z <= startPos.z)
        {
            moveCenterFL = false;
        }
        #endregion

        // move player from right to center
        #region Move Center From Right 
        // when the player is in position of the Right side
        if (verticalInput > 0 && transform.position.z < startPos.z && transform.position.z >= -verticalStep)
        {
            moveLeft = false;
            moveCenterFL = false;
            moveCenterFR = true;
            moveRight = false;
        }
        // only need a impulse input
        if (moveCenterFR)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPos, Time.deltaTime * verticalSpeed);
        }
        // When the player is in position 
        if (transform.position.z >= startPos.z)
        {
            moveCenterFR = false;
        }
        #endregion

        // move player from center to right
        #region Move Right 
        // when the player is in position of the center
        if (verticalInput < 0 && transform.position.z <= startPos.z && transform.position.z > -verticalStep)
        {
            moveLeft = false;
            moveCenterFL = false;
            moveCenterFR = false;
            moveRight = true;
        }
        // only need a impulse input
        if (moveRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPos, Time.deltaTime * verticalSpeed);
        }
        // When the player is in position 
        if (transform.position.z <= -verticalStep)
        {
            moveRight = false;
        }

        #endregion

    }
    
    //Limit player in x position
    void ConstrainPlayerPosition()
    {
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
    }
    #endregion

    #region Colliders
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true; //Is able to jump
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Enums.Pickups pickupType = other.gameObject.GetComponent<Pickups>().pickupType;            
            switch (pickupType)
            {
                case Enums.Pickups.Heal:
                    health += healPickup; //Just add a few extra health
                    if(health > maxHealth) //Not to go over the maxHealth
                    {
                        health = maxHealth;
                    }
                    inGameUI.UpdatePlayerHealth(); //Update Health in the UI
                    break;

                case Enums.Pickups.Invulnerability:
                    inGameUI.Invulnerability(pickupType); //Handle the powerup time in UI
                    invulnerable = true; //Is used in the applied damage section
                    break;

                case Enums.Pickups.Ball:
                    inGameUI.Balls(pickupType); //Handle the powerup time in UI
                    InvokeRepeating("SpawnBalls", 0.5f, 0.2f);
                    break;

                case Enums.Pickups.DoubleCoins:
                    inGameUI.DoubleCoins(pickupType); //Handle the powerup time in UI
                    bHasDoubleCoins = true; //Used in the UpdateScore function
                    break;

                case Enums.Pickups.Coin:
                    UpdateScore(coinValue);
                    break;

                case Enums.Pickups.Bomb:
                    Vector3 bombLocation = new Vector3(transform.position.x - 1.3f, transform.position.y + 2, transform.position.z);
                    spawnManager.SpawnBomb(bombLocation);
                    break;
            }
            other.gameObject.SetActive(false); //Disable the pickup
        }
    }
    #endregion

    #region Powerups

    public void SpawnBalls() //This function are used in InvokeRepeating
    { 
        Vector3 spawnPos = new Vector3(transform.position.x - 1, transform.position.y + 3, transform.position.z);
        spawnManager.SpawnBall(spawnPos);
    }

    public void StopPowerup()
    {
        switch (currentPowerup)
        {
            case Enums.Pickups.Invulnerability:
                invulnerable = false;
                break;
            case Enums.Pickups.Ball:
                CancelInvoke("SpawnBalls");
                break;
            case Enums.Pickups.DoubleCoins:
                bHasDoubleCoins = false;
                break;
        }
    }
    public void UpdateScore(int Amount)
    {
        if (bHasDoubleCoins)
        {
            Amount *= 2;
        }
        score += Amount;
        inGameUI.UpdateScore();
    }

    #endregion

    #region Damage + Death
    private void Death()
    {
        spawnManager.SpawnParticle(Enums.Particles.BlueLarge, gameObject.transform.position); //Spawn particle when dead
        gameObject.transform.position = new Vector3(0, -100, 0); //Place Player outside the viewport
        characterSelected.SetActive(false); //Set inactive to use again if user restarts game
        bIsDead = true;
    }

    public void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        if (invulnerable == false)
        {
            health -= damageTaken; //Calculate new health
            if (DamageDealt != null) 
            {
                DamageDealt(damageTaken); //Fire up the event
            }
            if (health <= 0 && !bIsDead) //Death if health below 0
            {
                GameManager.Instance.GameOver();
                Death();
            }
        }
        if(damageType == Enums.DamageType.Collision)
        {
            spawnManager.SpawnParticle(Enums.Particles.BlueSmall, damageLocation); //Spawn particle
        }

    }
    #endregion
}
