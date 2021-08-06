using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public delegate void ScoreUpdateHandler(float amount); //Create a delegate to use when score is updated
public class InGameUI : MonoBehaviour
{
    //public event DamageDealtHandler UpdateHealth;
    private PlayerController playerController;
    private BossMain bossMain;
    private int time = 0;
    public int count = 0;
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
            bossMain = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossMain>();
            bossHealthBar.maxValue = bossMain.maxHealth;
            bossHealthBar.value = bossMain.health;
            bossNameText.text = bossMain.BossName;
            bossHealthAmountDisplay.text = bossMain.health +"/" + bossMain.maxHealth;
            
            if (bossMain != null)
            {
                bossMain.DamageDealt += BossMain_DamageDealt; //Add function to method
            }            
        }
        if (currentState == GameManager.GameState.DEAD)
        {
            bossHealthBar.gameObject.SetActive(false);
            CancelInvoke();
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.DEAD)
        {
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
        bossHealthBar.value = bossMain.health;
        bossHealthAmountDisplay.text = bossMain.health + "/" + bossMain.maxHealth;
    }
    #endregion

    #region Updates
    public void UpdatePlayerHealth()
    {
        playerHealthBar.value = playerController.health;
        playerHealthAmountDisplay.text = playerController.health + "/" + playerController.maxHealth;
    }
    public void UpdateScore()
    {
        scoreText.text = "Score: " + playerController.score.ToString();
    }
    #endregion

    #region Powerups

    public void Invulnerability(Enums.Picups pickupType)
    {
        powerupIndicator.SetActive(true);
        powerupIndicatorTime[8].SetActive(true);
        powerupIndicator.GetComponent<Image>().sprite = invulnerablitySprite;
        StartCoroutine(PowerupCoroutine(pickupType));
    }

    public void FlameThrower(Enums.Picups pickupType)
    {
        powerupIndicator.SetActive(true);
        powerupIndicatorTime[8].SetActive(true);
        powerupIndicator.GetComponent<Image>().sprite = flameThrowerSprite;
        StartCoroutine(PowerupCoroutine(pickupType));
    }

    public void DoubleCoins(Enums.Picups pickupType)
    {
        powerupIndicator.SetActive(true);
        powerupIndicatorTime[8].SetActive(true);
        powerupIndicator.GetComponent<Image>().sprite = doubleCoinsSprite;
        StartCoroutine(PowerupCoroutine(pickupType));
    }
    public void Balls(Enums.Picups pickupType)
    {
        powerupIndicator.SetActive(true);
        powerupIndicatorTime[8].SetActive(true);
        powerupIndicator.GetComponent<Image>().sprite = ballSprite;
        StartCoroutine(PowerupCoroutine(pickupType));
    }

    IEnumerator PowerupCoroutine(Enums.Picups pickupType)
    {
        count = 7;
        while (count > -1)
        {
            WaitForSeconds waitTime = new WaitForSeconds(1);
            yield return waitTime;
            DeactivatePowerupIndicatorTime();
            powerupIndicatorTime[count].SetActive(true);
            count--;
        }
        DeactivatePowerupIndicatorTime();
        powerupIndicator.SetActive(false);
        playerController.StopPowerup(pickupType);
    }


    private void DeactivatePowerupIndicatorTime()
    {
        for (int i = 0; i<powerupIndicatorTime.Count; i++)
        {
            powerupIndicatorTime[i].SetActive(false);
        }
    }

    #endregion

}
