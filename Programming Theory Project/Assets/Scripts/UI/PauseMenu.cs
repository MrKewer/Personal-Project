using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject optionsScreen; //This screen has its own script

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button optionsBackButton;
    [SerializeField] private Button exitButton;

    private void OnEnable()
    {
        ShowPauseMenu();
    }
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(OptionsButtonClicked);
        optionsBackButton.onClick.AddListener(ShowPauseMenu);
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

    private void OptionsButtonClicked()
    {
        DeactivateAll();
        optionsScreen.SetActive(true);
        
    }
    private void ShowPauseMenu()
    {
        DeactivateAll();
        pauseScreen.SetActive(true);
    }
    private void DeactivateAll()
    {
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(false);
    }
}
