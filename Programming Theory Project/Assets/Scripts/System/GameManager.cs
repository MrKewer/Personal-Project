using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


//UnityEvent<Current GameState, Previous GameState>
//Serializable in order to register via the inspecter
[System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LoadSave loadSave; //Reference to the LoadSave script
    public List<HighScoreList> highScores = new List<HighScoreList>();
    public float gameSpeed = 10f; //The speed at which the game will run
    public int levelSelectedNumber = 2; //The level the player will choose in the MainMenu
    public int characterSelectedNumber = 0; //The Character the player will choose in the MainMenu
    public string playerName = "a"; //The Name the player will type in, in the MainMenu
        
    public int enemiesDead = 0; //Keeps count of the enemies killed
    public int enemiesDeadBeforeBossFight = 2; //The amount of enemies to kill before the boss comes out
    public int maxSpawnedEnemies = 10; //The max amount of active enemies in level
    public int basicEnemiesCount = 0; //The count of active enemies

    [SerializeField] private float gravityModifier = 10f; //To make a nicer jump for the player

    public EventGameState OnGameStateChanged; //Create event to know when the game state changes
    public GameObject[] SystemPrefabs; //The list of all the Managers you want to create. (UI, Sound, Gameplay ect)
    private List<GameObject> _instancedSystemPrefabs; //A list to keep track of all the Managers: Create, Destroy

    List<AsyncOperation> _loadOperations; //To keep track of the operations
                                          //To be able to load alot of levels and to know if all the loads are completed
                                          //Or prevent other levels to be loaded at the same time

    [SerializeField] private string _currentLevelName = string.Empty; //The current level selected

    #region Game States
    public enum GameState //Enumerating the states
    {
        MAINMENU,
        RUNNING,
        BOSSFIGHT,
        ENDGAME,
        PAUSED,
        DEAD
    }
    [SerializeField] GameState _currentGameState = GameState.MAINMENU; //Setup the Current State
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

            case GameState.BOSSFIGHT:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            case GameState.ENDGAME:
                Time.timeScale = 0.5f;
                break;

            case GameState.DEAD:
                Time.timeScale = 0.5f;
                break;

            default:
                break;
        }
        //Invoke method is used to distinguish between events and methods located inside of a class
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }
    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject); //Called to keep this script even if it loads a complete new level
        loadSave = gameObject.GetComponent<LoadSave>(); //Gets reference to the LoadSave script
        LoadGame(); //Load game on startup
        Physics.gravity *= gravityModifier; //This should only be done once in the game
        _loadOperations = new List<AsyncOperation>(); //Initialize the list
        _instancedSystemPrefabs = new List<GameObject>(); //Initialize the list
        InstantiateSystemPrefabs(); //Create all the managers
    }

    // Update is called once per frame
    void Update()
    {
        //When the game is running or if it is paused
        if (Input.GetKeyDown(KeyCode.Escape) && (_currentGameState == GameState.RUNNING || _currentGameState == GameState.PAUSED || _currentGameState == GameState.BOSSFIGHT))
        {
            TogglePause(); //Will swich between pause and unpause
        }
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
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName); //Unload the named scene
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete; //Add a listener for when the unload is complete
    }
    void OnLoadOperationComplete(AsyncOperation ao) //Needs a AsyncOpertation
    {
        if (_loadOperations.Contains(ao)) //To check if the operation does exist
        {
            _loadOperations.Remove(ao); //Remove the operation form the list

            if (_loadOperations.Count == 0) // When the level load is complete
            {

                UpdateState(GameState.RUNNING); // Asuming that the game only has the one gameplay type

            }
        }
        //Debug.Log("Load Complete.");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        //Debug.Log("Unload Complete.");
    }
    public void StartGame() //Call to start the game
    {
        LoadLevel("Game");
    }

    #endregion

    #region Create + Destroy Managers
    void InstantiateSystemPrefabs()// Create all the Managers
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]); //Create the Manager
            _instancedSystemPrefabs.Add(prefabInstance); //Add the Manager
        }
    }
    protected override void OnDestroy() //Override the Destroy Method form Singleton script
    {
        base.OnDestroy(); //Call the same destroy script
        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i) //Destroy all the managers created
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear(); //Clear the list
    }
    #endregion

    #region Game State Functions
    public void RestartGame()
    {
        enemiesDead = 0;
        UpdateState(GameState.RUNNING);
    }
    public void ResumeGame()
    {
        UpdateState(GameState.RUNNING);
    }
    public void ExitToMain()
    {
        enemiesDead = 0;
        UnloadLevel(_currentLevelName);
        UpdateState(GameState.MAINMENU);
    }
    public void GameOver()
    {
        enemiesDead = 0;
        UpdateState(GameState.DEAD);
    }
    public void EndGame()
    {
        enemiesDead = 0;
        UpdateState(GameState.ENDGAME);
    }
    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void BossFight()
    {
        UpdateState(GameState.BOSSFIGHT);
    }
    #endregion

    #region LoadSave
    private void LoadGame()
    {
        loadSave.LoadGame(); //To the loading on the LoadSave Script
    }
    public void SaveGame(int score, int time)
    {
        //convert the time to time format
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        //use the 6th one to add a new score, this one will not be displayed, only used then to sort out the list
        highScores[5] = new HighScoreList(playerName, score, "Level" + (levelSelectedNumber+1), string.Format("{0:00}:{1:00}", minutes, seconds));

        highScores.Sort(); //Sort out list with new item added to list
        loadSave.SaveGame(); //Save the new list
    }
    #endregion

}
