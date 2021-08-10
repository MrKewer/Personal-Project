using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR //Only editor can read this
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{

    //UI screens inside the Main Menu
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject enterName;
    [SerializeField] private GameObject characterSelect;
    [SerializeField] private GameObject levelSelect;

    [Space]
    [Header("Title Screen")]
    [Space]
    //The buttons on the title screen
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button highScoreButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    [Space]
    [Header("Player Name Screen")]
    [Space]
    //Enter Name Screen
    [SerializeField] private InputField playerName;
    [SerializeField] private Button enterNameNextButton;
    [SerializeField] private Button enterNameBackButton;

    [Space]
    [Header("Character Select Screen")]
    [Space]
    //Character Select Screen
    [SerializeField] private Button characterSelectLeftScrollButton;
    [SerializeField] private Button characterSelectRightScrollButton;
    [SerializeField] private Button characterSelectNextButton;
    [SerializeField] private Button characterSelectBackButton;
    [SerializeField] private GameObject characterListPrefab; //The list of all the playable characters
    private List<GameObject> characterList; //List from characterListPrefab
    private List<GameObject> characterPool = new List<GameObject>(); //Created Game Objects, that will be displayed
    [SerializeField] private int characterNumber = 0; //Current character in the list

    [Space]
    [Header("Level Select Screen")]
    [Space]
    //Level Select Screen
    [SerializeField] private Button levelSelectLeftScrollButton;
    [SerializeField] private Button levelSelectRightScrollButton;
    [SerializeField] private Button levelSelectStartButton;
    [SerializeField] private Button levelSelectBackButton;
    [SerializeField] private Image level; //The displayed level sprite
    [SerializeField] private GameObject levelListPrefab; //The list of all the levels
    private List<Sprite> levelList; //The list from the levelListPrefab
    [SerializeField] private int levelNumber = 0; //Current Level in the list


    [Space]
    [Header("HighScore Screen")]
    [Space]
    //The buttons on the title screen
    [SerializeField] private Button highScoreBackButton;

    [Space]
    [Header("Options Screen")]
    [Space]
    //The buttons on the title screen

    [SerializeField] private Button optionsBackButton;

    private void OnEnable()
    {
        TitleScreen(); //The fist screen to show
    }
    void Start()
    {
        characterList = characterListPrefab.GetComponent<CharacterList>().characterList; //Gets the list of the characters
        levelList = levelListPrefab.GetComponent<LevelList>().levelSelectList; //Gets the list of the levels
        PoolCharacters(); //Pool the characters
        AddListeners(); //All the button listeners
    }

    void AddListeners()//All the button listeners
    {
        //Title Screen
        newGameButton.onClick.AddListener(NewGame);
        highScoreButton.onClick.AddListener(HighScore);
        optionButton.onClick.AddListener(Options);
        exitButton.onClick.AddListener(ExitGame);

        //Enter Name Screen
        enterNameNextButton.onClick.AddListener(CharacterSelectScreen);
        enterNameBackButton.onClick.AddListener(TitleScreen);
        
        //Character Select Screen
        characterSelectLeftScrollButton.onClick.AddListener(CharacterSelectScrollLeft);
        characterSelectRightScrollButton.onClick.AddListener(CharacterSelectScrollRight);
        characterSelectNextButton.onClick.AddListener(LevelSelectScreen);
        characterSelectBackButton.onClick.AddListener(EnterNameScreen);

        //Level Select Screen
        levelSelectLeftScrollButton.onClick.AddListener(LevelSelectScrollLeft);
        levelSelectRightScrollButton.onClick.AddListener(LevelSelectScrollRight);
        levelSelectStartButton.onClick.AddListener(StartGame);
        levelSelectBackButton.onClick.AddListener(CharacterSelectScreen);

        //High Score Screen
        highScoreBackButton.onClick.AddListener(TitleScreen);

        //Options Screen
        optionsBackButton.onClick.AddListener(TitleScreen);
    }

    #region Title Screen
    private void TitleScreen()
    {
        DeactivateAllScreens();
        titleScreen.SetActive(true);
    }
    #region New Game
    private void NewGame()
    {
        ClearInfo();
        EnterNameScreen();
    }
    private void ClearInfo()
    {
        playerName.text = "";
        characterNumber = 0;
        levelNumber = 0;
        level.sprite = levelList[levelNumber];
        CharacterSelectDeactivatePool();
        characterPool[characterNumber].SetActive(true);
    }
    #endregion

    #endregion

    #region Enter Name Screen
    private void EnterNameScreen()
    {
        CharacterSelectDeactivatePool();
        DeactivateAllScreens();
        enterName.SetActive(true);
    }

    #endregion

    #region Character Select

    void PoolCharacters() //Create characters + set their position
    {
        Vector3 characterPosition = new Vector3(0, 0, 0);
        Quaternion characterRotation = Quaternion.Euler(0, 126, 0);

        for (int i = 0; i < characterList.Count; i++)
        {
            GameObject pooledCharacter = Instantiate(characterList[i], characterPosition, characterRotation);
            pooledCharacter.SetActive(false);
            characterPool.Add(pooledCharacter);
        }
    }
    private void CharacterSelectDeactivatePool() //Set all characters inActive
    {
        for (int i = 0; i < characterPool.Count; i++)
        {
            characterPool[i].SetActive(false);
        }
    }
    private void CharacterSelectScreen()
    {
        if (playerName.text != "")
        {
            DeactivateAllScreens();
            characterSelect.SetActive(true);
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
    }
    private void CharacterSelectScrollRight() //Scroll right
    {
        characterNumber++;        
        if (characterNumber <= characterList.Count -1)
        {
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
        else //Loop the character selection
        {
            characterNumber = 0;
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
    }
    private void CharacterSelectScrollLeft() //Scroll Left
    {
        characterNumber--;        
        if (characterNumber >= 0)
        {
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
        else //Loop the character selection
        {
            characterNumber = characterList.Count - 1;
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
    }
    #endregion

    #region Level Select
    private void LevelSelectScreen()
    {
        CharacterSelectDeactivatePool();
        DeactivateAllScreens();
        levelSelect.SetActive(true);
    }
    private void LevelSelectScrollRight() //Scroll Right
    {
        levelNumber++;
        if (levelNumber <= levelList.Count - 1)
        {
            level.sprite = levelList[levelNumber];
        }
        else //Max level Number
        {
            levelNumber = levelList.Count - 1;
        }
    }
    private void LevelSelectScrollLeft() //Scroll Left
    {
        levelNumber--;
        if (levelNumber >= 0)
        {
            level.sprite = levelList[levelNumber];
        }
        else //Min level number
        {
            levelNumber = 0;
        }
    }

    #endregion

    #region HighScore Screen
    private void HighScore()
    {
        DeactivateAllScreens();
        highScoreScreen.SetActive(true);
    }

    #endregion

    #region Options Screen
    private void Options()
    {
        DeactivateAllScreens();
        optionsScreen.SetActive(true);
    }

    #endregion



    private void StartGame()
    {
        GameManager.Instance.playerName = playerName.text;
        GameManager.Instance.characterSelectedNumber = characterNumber;
        GameManager.Instance.levelSelectedNumber = levelNumber;
        GameManager.Instance.StartGame();
    }

    private void DeactivateAllScreens()
    {
        titleScreen.SetActive(false);
        enterName.SetActive(false);
        characterSelect.SetActive(false);
        levelSelect.SetActive(false);
        optionsScreen.SetActive(false);
        highScoreScreen.SetActive(false);
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
Application.Quit();
#endif
    }
}
