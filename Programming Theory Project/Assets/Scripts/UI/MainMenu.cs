using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject enterName;
    [SerializeField] private GameObject characterSelect;
    [SerializeField] private GameObject levelSelect;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private InputField playerName;
    [SerializeField] private Button enterNameNextButton;
    [SerializeField] private Button enterNameBackButton;

    [SerializeField] private Button characterSelectLeftScrollButton;
    [SerializeField] private Button characterSelectRightScrollButton;
    [SerializeField] private Button characterSelectNextButton;
    [SerializeField] private Button characterSelectBackButton;

    [SerializeField] private Button levelSelectLeftScrollButton;
    [SerializeField] private Button levelSelectRightScrollButton;
    [SerializeField] private Button levelSelectStartButton;
    [SerializeField] private Button levelSelectBackButton;

    //[SerializeField] private GameObject character;
    [SerializeField] private Image level;
    [SerializeField] private GameObject levelListPrefab;
    private List<Sprite> levelList;
    [SerializeField] private GameObject characterListPrefab;
    private List<GameObject> characterList;
    private List<GameObject> characterPool = new List<GameObject>();
    [SerializeField] private int levelNumber = 0;
    [SerializeField] private int characterNumber = 0;
    // Start is called before the first frame update



    void Start()
    {
        TitleScreen();
        characterList = new List<GameObject>(characterListPrefab.GetComponent<CharacterList>().characterList);
        levelList = new List<Sprite>(levelListPrefab.GetComponent<LevelList>().levelSelectList);
        PoolCharacters();
        newGameButton.onClick.AddListener(EnterNameScreen);
        exitButton.onClick.AddListener(ExitGame);

        enterNameNextButton.onClick.AddListener(CharacterSelectScreen);
        enterNameBackButton.onClick.AddListener(TitleScreen);

        characterSelectLeftScrollButton.onClick.AddListener(CharacterSelectScrollLeft);
        characterSelectRightScrollButton.onClick.AddListener(CharacterSelectScrollRight);
        characterSelectNextButton.onClick.AddListener(LevelSelectScreen);
        characterSelectBackButton.onClick.AddListener(EnterNameScreen);

        levelSelectLeftScrollButton.onClick.AddListener(LevelSelectScrollLeft);
        levelSelectRightScrollButton.onClick.AddListener(LevelSelectScrollRight);
        levelSelectStartButton.onClick.AddListener(StartGame);
        levelSelectBackButton.onClick.AddListener(CharacterSelectScreen);
    

    }

    #region Title Screen
    private void TitleScreen()
    {
        DeactivateAllScreens();
        titleScreen.SetActive(true);
    }
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

    void PoolCharacters()
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
    private void CharacterSelectDeactivatePool()
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
    private void CharacterSelectScrollRight()
    {
        characterNumber++;        
        if (characterNumber <= characterList.Count -1)
        {
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
        else
        {
            characterNumber = 0;
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
    }
    private void CharacterSelectScrollLeft()
    {
        characterNumber--;        
        if (characterNumber >= 0)
        {
            CharacterSelectDeactivatePool();
            characterPool[characterNumber].SetActive(true);
        }
        else
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
    private void LevelSelectScrollLeft()
    {
        levelNumber--;
        if (levelNumber >= 0)
        {
            level.sprite = levelList[levelNumber];
        }
        else
        {
            levelNumber = 0;
        }
    }
    private void LevelSelectScrollRight()
    {
        levelNumber++;
        if (levelNumber <= levelList.Count - 1)
        {
            level.sprite = levelList[levelNumber];
        }
        else
        {
            levelNumber = levelList.Count - 1;
        }
    }
    #endregion



    private void StartGame()
    {

    }
    private void DeactivateAllScreens()
    {
        titleScreen.SetActive(false);
        enterName.SetActive(false);
        characterSelect.SetActive(false);
        levelSelect.SetActive(false);
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
