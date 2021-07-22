using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
public class GameManager : Singleton<GameManager>
{
    public int levelSelectedNumber = 2;
    public int characterSelectedNumber = 0;
    public string playerName = "a";

    public EventGameState OnGameStateChanged; //Create event to know when the game state changes
    public GameObject[] SystemPrefabs; //The list of all the Managers you want to create. (UI, Sound, Gameplay ect)
    private List<GameObject> _instancedSystemPrefabs; //A list to keep track of all the Managers: Create, Destroy

    List<AsyncOperation> _loadOperations; //To keep track of the operations
                                          //To be able to load alot of levels and to know if all the loads are completed
                                          //Or prevent other levels to be loaded at the same time

    private string _currentLevelName = string.Empty;

    public enum GameState //Enumerating the states
    {
        MAINMENU,
        RUNNING,
        PAUSED,
        DEAD
    }

    GameState _currentGameState = GameState.MAINMENU; //Setup the Current State
    public GameState CurrentGameState //If you want to know what the current game state is
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    void UpdateState(GameState state) //Updates the state
    {
        GameState previousGameState = _currentGameState; //Get the previous State
        _currentGameState = state;
        switch (_currentGameState)
        {
            case GameState.MAINMENU:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f; 
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f; 
                break;
            case GameState.DEAD:
                Time.timeScale = 1.0f;
                break;

            default:
                break;
        }
        //Invoke method is used to distinguish between events and methods located inside of a class
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject); //Called to keep this script even if it loads a complete new level
        _loadOperations = new List<AsyncOperation>(); //Initialize the list
        _instancedSystemPrefabs = new List<GameObject>(); //Initialize the list
        InstantiateSystemPrefabs(); //Create all the managers
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Level Load
    public void LoadLevel(string levelName)
    {
        AsyncOperation ao //Returns a opertaion to let you know when its done
            = SceneManager.LoadSceneAsync //LoadScene(stops everything) vs LoadSceneAsync
            (levelName, LoadSceneMode.Additive); //Add the scene to current scene (to keep the GameManager)
        if (ao == null)//If the level does not exist, or for other errors
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return; //Exit out of the method
        }
        ao.completed += OnLoadOperationComplete; //Add a listener for when the load is completed
        _loadOperations.Add(ao); //Adding this loading operation to the list

        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }
    void OnLoadOperationComplete(AsyncOperation ao) //Needs a AsyncOpertation
    {
        if (_loadOperations.Contains(ao)) //To check if the operation does exist
        {
            _loadOperations.Remove(ao); //Remove the operation form the list

            if (_loadOperations.Count == 0) //5.3 - When the level load is complete
            {
                UpdateState(GameState.RUNNING);
            }
        }
        Debug.Log("Load Complete.");
    }
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }

    public void StartGame() //5.3 - Call to start the game
    {
        LoadLevel("Game");
    }

    #endregion

    void InstantiateSystemPrefabs() //3.3 - Create all the Managers
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]); //3.3 - Create the Manager
            _instancedSystemPrefabs.Add(prefabInstance); //3.3 - Add the Manager
        }
    }
}
