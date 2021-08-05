using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    //public event DamageDealtHandler UpdateHealth;
    private PlayerController playerController;
    private BossMain bossMain;
    [SerializeField] private TextMeshProUGUI playerNameText; //The text to display player name
    [SerializeField] private Slider playerHealthBar; //The health bar of the player
    [SerializeField] private TextMeshProUGUI playerHealthAmountDisplay; //The health bar of the player
    [SerializeField] private TextMeshProUGUI scoreText; //The text of the displayed score
    [SerializeField] private TextMeshProUGUI timeText; //The text to display time
    private int time = 0;
    [SerializeField] private GameObject powerupIndicator; //Indicator of the powerup picked up
    [SerializeField] private List<GameObject> powerupIndicatorTime; //Timer of the powerup

    [SerializeField] private TextMeshProUGUI bossNameText; // The name of the boss
    [SerializeField] private Slider bossHealthBar; //The health bar of the boss
    [SerializeField] private TextMeshProUGUI bossHealthAmountDisplay; //The health bar of the player

    private void OnEnable()
    {
        //Set all equal to the player's input in the MainMenu
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        playerNameText.text = GameManager.Instance.playerName;
        playerHealthBar.maxValue = playerController.maxHealth;
        playerHealthBar.value = playerController.health;
        scoreText.text = "Score: " + playerController.score.ToString();
        playerController.DamageDealt += PlayerController_DamageDealt;
        playerHealthAmountDisplay.text = playerController.health + "/" + playerController.maxHealth;
    }

    private void OnDisable()
    {
        playerController.DamageDealt -= PlayerController_DamageDealt;
    }
    void Start()
    {
        InvokeRepeating("Timer", 0, 1);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //When the state changes in the GameManager
    {
        if (currentState == GameManager.GameState.BOSSFIGHT)
        {
            bossHealthBar.gameObject.SetActive(true);
            bossMain = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossMain>();
            bossHealthBar.maxValue = bossMain.maxHealth;
            bossHealthBar.value = bossMain.health;
            bossNameText.text = bossMain.BossName;
            bossHealthAmountDisplay.text = bossMain.health +"/" + bossMain.maxHealth;

            if (bossMain != null)
            {
                bossMain.DamageDealt += BossMain_DamageDealt;
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
    void Timer()
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        time += 1;
    }

    private void PlayerController_DamageDealt(float amount)
    {
        playerHealthBar.value = playerController.health;
        playerHealthAmountDisplay.text = playerController.health + "/" + playerController.maxHealth;
    }
    private void BossMain_DamageDealt(float amount)
    {
        bossHealthBar.value = bossMain.health;
        bossHealthAmountDisplay.text = bossMain.health + "/" + bossMain.maxHealth;
    }
}
