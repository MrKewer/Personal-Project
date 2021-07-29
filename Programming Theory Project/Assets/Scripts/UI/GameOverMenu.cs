using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitToMain);
    }

    void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    void ExitToMain()
    {
        GameManager.Instance.ExitToMain();
    }
}
