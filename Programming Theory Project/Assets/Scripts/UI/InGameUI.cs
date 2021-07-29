using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    //public event DamageDealtHandler UpdateHealth;
    private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI playerNameText; //The text to display player name
    [SerializeField] private Slider playerHealthBar; //The health bar of the player
    [SerializeField] private TextMeshProUGUI scoreText; //The text of the displayed score
    [SerializeField] private TextMeshProUGUI timeText; //The text to display time
    private int seconds;
    private int minutes;
    [SerializeField] private GameObject powerupIndicator; //Indicator of the powerup picked up
    [SerializeField] private List <GameObject> powerupIndicatorTime; //Timer of the powerup

    [SerializeField] private TextMeshProUGUI bossNameText; // The name of the boss
    [SerializeField] private Slider bossHealthBar; //The health bar of the boss

    private void OnEnable()
    {
        //Set all equal to the player's input in the MainMenu
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerNameText.text = GameManager.Instance.playerName;
        playerHealthBar.maxValue = playerController.maxHealth;
        playerHealthBar.value = playerController.health;
        scoreText.text = "Score: " + playerController.score.ToString();
        playerController.DamageDealt += PlayerController_DamageDealt;
    }
    private void OnDisable()
    {
        playerController.DamageDealt -= PlayerController_DamageDealt;
    }
    void Start()
    {
        
    }

    private void PlayerController_DamageDealt(float amount)
    {
        playerHealthBar.value = playerController.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossFight() 
    {
    
    }
}
