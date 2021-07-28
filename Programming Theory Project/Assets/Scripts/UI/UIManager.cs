using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private EndGameMenu _endGameMenu;
    [SerializeField] private GameOverMenu _gameOverMenu;
    [SerializeField] private Camera mainMenuCamera; //This camera is only used for the Main Menu display
    void Start()
    {
        DontDestroyOnLoad(gameObject); //Called to keep this script even if it loads a complete new level
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged); //Add a Listener to the event
        _pauseMenu.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED); //If the game is paused show the UI
        _mainMenu.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.MAINMENU);
        mainMenuCamera.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.MAINMENU);
        _inGameUI.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING);
        _endGameMenu.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.ENDGAME);
        _gameOverMenu.gameObject.SetActive(GameManager.Instance.CurrentGameState == GameManager.GameState.DEAD);
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) //
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED); //If the game is paused show the UI
        _mainMenu.gameObject.SetActive(currentState == GameManager.GameState.MAINMENU);
        mainMenuCamera.gameObject.SetActive(currentState == GameManager.GameState.MAINMENU);
        _inGameUI.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        _endGameMenu.gameObject.SetActive(currentState == GameManager.GameState.ENDGAME);
        _gameOverMenu.gameObject.SetActive(currentState == GameManager.GameState.DEAD);
    }

    

}
