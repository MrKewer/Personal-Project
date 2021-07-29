using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToMain);
    }

    void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    void ExitToMain()
    {
        GameManager.Instance.ExitToMain();
    }
}
