using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private List <GameObject> powerupIndicatorTime;

    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Slider bossHealthBar;

    private void OnEnable()
    {
        playerNameText.text = GameManager.Instance.playerName;
        playerHealthBar.maxValue = 100;
        playerHealthBar.value = 100;
        scoreText.text = "Score: 1";

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossFight() { }
}
