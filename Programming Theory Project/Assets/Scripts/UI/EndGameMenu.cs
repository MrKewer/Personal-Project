using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameMenu : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreText.text = playerController.score.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitToMain);
        restartButton.onClick.AddListener(RestartGame);
    }

    void ExitToMain()
    {
        GameManager.Instance.ExitToMain();
    }

    void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
