using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void DamageDealtHandler(float amount);
public class PlayerController : MonoBehaviour, IDamageable<float, string, Vector3>
{
    public event DamageDealtHandler DamageDealt;
    private GameObject characterSelected;
    [SerializeField] private GameObject CharacterListPrefab;
    private Animator runAnimation;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float verticalSpeed = 10.0f;
    [SerializeField] private float verticalStep = 3f;
    [SerializeField] private float xBound = 10.0f;
    [SerializeField] private float jumpForce = 250.0f;
    [SerializeField] private float gravityModifier = 10f;
    [SerializeField] private float tolerance = 0;
    [SerializeField] private bool isOnGround;
    [SerializeField] private bool moveLeft = false;
    [SerializeField] private bool moveCenterFL = false;
    [SerializeField] private bool moveCenterFR = false;
    [SerializeField] private bool moveRight = false;

    public bool invulnerable = false;
    public float maxHealth = 100f;
    public float health = 100f;
    public int score = 0;
    private Rigidbody playerRb;
    private Vector3 startPos;
    [SerializeField] private GameObject Explode;
    [SerializeField] private SpawnManager spawnManager;


    public float VerticalStep
    {
        get { return verticalStep; }
        private set { }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event

        Vector3 characterTransform = new Vector3(0, 0, 0);
        Quaternion characterRotation = Quaternion.Euler(0, 90, 0);
        characterSelected = CharacterListPrefab.GetComponent<CharacterList>().characterList[GameManager.Instance.characterSelectedNumber];
        characterSelected = Instantiate(characterSelected, characterTransform, characterRotation);
        characterSelected.transform.SetParent(gameObject.transform);

        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        startPos = transform.position;
        resetAll();
        //runAnimation = GetComponentInChildren<Animator>();
        runAnimation = characterSelected.GetComponent<Animator>();
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.DEAD)
        {
            resetAll();
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.DEAD)
        {
            characterSelected.SetActive(true);
        }
    }
    private void resetAll()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        moveLeft = false;
        moveCenterFL = false;
        moveCenterFR = false;
        moveRight = false;
        health = maxHealth;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayerPosition();

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    // Player Movement
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
        if (verticalInput > 0 && transform.position.z >= startPos.z - tolerance && transform.position.z < verticalStep)
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
        if (verticalInput < 0 && transform.position.z > startPos.z - tolerance && transform.position.z <= verticalStep)
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
        if (transform.position.z <= startPos.z + tolerance)
        {
            moveCenterFL = false;
        }
        #endregion

        // move player from right to center
        #region Move Center From Right 
        // when the player is in position of the Right side
        if (verticalInput > 0 && transform.position.z < startPos.z + tolerance && transform.position.z >= -verticalStep)
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
        if (transform.position.z >= startPos.z - tolerance)
        {
            moveCenterFR = false;
        }
        #endregion

        // move player from center to right
        #region Move Right 
        // when the player is in position of the center
        if (verticalInput < 0 && transform.position.z <= startPos.z + tolerance && transform.position.z > -verticalStep)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Debug.Log("Powerup has been triggered");
            Destroy(other.gameObject);
        }
    }
    private void Death()
    {
        Instantiate(Explode, gameObject.transform.position, Explode.transform.rotation);
        characterSelected.SetActive(false);
    }

    private void OnDestroy()
    {
        Physics.gravity /= gravityModifier;
    }

    public void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        if (invulnerable == false)
        {
            health -= damageTaken;
            if (DamageDealt != null)
            {
                DamageDealt(damageTaken);
            }
            if (health <= 0)
            {
                GameManager.Instance.GameOver();
                Death();
            }
        }
        if(damageType == "Collision")
        {
            GameObject particalEffect = spawnManager.GetAvailableYellowSmallPartical();
            particalEffect.SetActive(true);
            particalEffect.transform.position = damageLocation;
        }

    }
}
