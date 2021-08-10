using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public delegate void ScoreUpdateHandler(float amount); //Create a delegate to use when score is updated
public class InGameUI : MonoBehaviour
{
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private BossMain bossMain;
    private int time = 0;
    public int count = 0;
    private bool bHasPowerup = false;

    [SerializeField] private TextMeshProUGUI playerNameText; //The text to display player name
    [SerializeField] private Slider playerHealthBar; //The health bar of the player
    [SerializeField] private TextMeshProUGUI playerHealthAmountDisplay; //The health bar of the player
    [SerializeField] private TextMeshProUGUI scoreText; //The text of the displayed score
    [SerializeField] private TextMeshProUGUI timeText; //The text to display time


    [SerializeField] private TextMeshProUGUI bossNameText; // The name of the boss
    [SerializeField] private Slider bossHealthBar; //The health bar of the boss
    [SerializeField] private TextMeshProUGUI bossHealthAmountDisplay; //The health bar of the player
    [Space]
    [Header("Pickup Indicator")]
    [Space]
    [SerializeField] private List<GameObject> powerupIndicatorTime; //Timer of the powerup
    [SerializeField] private GameObject powerupIndicator; //Indicator of the powerup picked up
    [SerializeField] private Sprite invulnerablitySprite;
    [SerializeField] private Sprite ballSprite;
    [SerializeField] private Sprite doubleCoinsSprite;
    [SerializeField] private Sprite flameThrowerSprite;



    private void OnEnable()
    {
        //Set all equal to the player's input in the MainMenu

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerNameText.text = GameManager.Instance.playerName;
        playerHealthBar.maxValue = playerController.maxHealth;
        UpdatePlayerHealth();
        UpdateScore();
        playerController.DamageDealt += PlayerController_DamageDealt;
        playerController.inGameUI = this;
    }

    private void OnDisable()
    {
        playerController.DamageDealt -= PlayerController_DamageDealt; //Remove call from event
    }
    void Start()
    {
        InvokeRepeating("Timer", 0, 1); //InvokeRepeating, is said to be the most acurate timer
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.BOSSFIGHT) //When the bossfight begins
        {
            bossHealthBar.gameObject.SetActive(true);
            bossMain = spawnManager.bossesPool[0].GetComponent<BossMain>();
            //bossMain = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossMain>();
            bossHealthBar.maxValue = bossMain.maxHealth;
            bossNameText.text = bossMain.BossName;
            UpdateBossHealth();

            if (bossMain != null)
            {
                bossMain.DamageDealt += BossMain_DamageDealt; //Add function to method
            }
        }
        if (currentState == GameManager.GameState.DEAD)
        {
            bossHealthBar.gameObject.SetActive(false);
            PowerupCountComplete();
            CancelInvoke();
        }
        if (currentState == GameManager.GameState.ENDGAME && (previousState == GameManager.GameState.BOSSFIGHT || previousState == GameManager.GameState.RUNNING))
        {
            bossHealthBar.gameObject.SetActive(false);
            PowerupCountComplete();
            CancelInvoke();
            GameManager.Instance.SaveGame(playerController.score, time);
        }

        if (currentState == GameManager.GameState.RUNNING && (previousState == GameManager.GameState.DEAD || previousState == GameManager.GameState.ENDGAME || previousState == GameManager.GameState.MAINMENU))
        {
            bossHealthBar.gameObject.SetActive(false);
            bossMain = null;
            CancelInvoke();
            time = 0;
            InvokeRepeating("Timer", 0, 1);
        }
    }

    void Timer() //The timer to display time
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        time += 1;
    }

    #region Events
    private void PlayerController_DamageDealt(float amount) //Added to event when the player received damage
    {
        UpdatePlayerHealth();
    }
    private void BossMain_DamageDealt(float amount) //Added to event when the boss received damage
    {
        UpdateBossHealth();
    }
    #endregion

    #region Updates

    public void UpdateBossHealth()
    {
        bossHealthBar.value = bossMain.health;
        bossHealthAmountDisplay.text = bossMain.health + "/" + bossMain.maxHealth;
    }
    public void UpdatePlayerHealth()
    {
        playerHealthBar.value = playerController.health;
        playerHealthAmountDisplay.text = playerController.health + "/" + playerController.maxHealth;
    }
    public void UpdateScore()
    {
        scoreText.text = playerController.score.ToString();
    }
    #endregion

    #region Powerups

    public void Invulnerability(Enums.Pickups pickupType)
    {
        StartPowerup(pickupType);
        powerupIndicator.GetComponent<Image>().sprite = invulnerablitySprite;
    }

    public void FlameThrower(Enums.Pickups pickupType)
    {
        StartPowerup(pickupType);
        powerupIndicator.GetComponent<Image>().sprite = flameThrowerSprite;
    }

    public void DoubleCoins(Enums.Pickups pickupType)
    {
        StartPowerup(pickupType);
        powerupIndicator.GetComponent<Image>().sprite = doubleCoinsSprite;
    }
    public void Balls(Enums.Pickups pickupType)
    {
        StartPowerup(pickupType);
        powerupIndicator.GetComponent<Image>().sprite = ballSprite;
    }
    private void StartPowerup(Enums.Pickups pickupType)
    {
        if (bHasPowerup)
        {
            PowerupCountComplete();
        }
        powerupIndicator.SetActive(true);
        powerupIndicatorTime[8].SetActive(true);
        playerController.currentPowerup = pickupType;
        count = 8;
        InvokeRepeating("PowerupCounter", 0, 1);
    }
    private void PowerupCounter()
    {
        bHasPowerup = true;
        DeactivatePowerupIndicatorTime();
        powerupIndicatorTime[count].SetActive(true);
        count--;
        if (count == -1)
        {
            PowerupCountComplete();
        }
    }
    private void PowerupCountComplete()
    {
        DeactivatePowerupIndicatorTime();
        bHasPowerup = false;
        powerupIndicator.SetActive(false);
        playerController.StopPowerup();
        CancelInvoke("PowerupCounter");
    }

    private void DeactivatePowerupIndicatorTime()
    {
        for (int i = 0; i < powerupIndicatorTime.Count; i++)
        {
            powerupIndicatorTime[i].SetActive(false);
        }
    }

    #endregion

}
