using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject enterName;
    [SerializeField] private GameObject characterSelect;
    [SerializeField] private GameObject levelSelect;

    [SerializeField] private Button newGameButton;

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

    [SerializeField] private GameObject character;
    [SerializeField] private GameObject level;
    [SerializeField] private List<Sprite> levelList;
    [SerializeField] private List<GameObject> characterList;
    [SerializeField] private int levelNumber = 0;
    [SerializeField] private int characterNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        TitleScreen();
        newGameButton.onClick.AddListener(EnterNameScreen);
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

    private void CharacterSelectScrollRight()
    {
        characterNumber++;
        if(characterNumber <= characterList.Count -1)
        {
            character = characterList[characterNumber];
        }
        else
        {
            characterNumber = characterList.Count - 1;
        }
    }
    private void CharacterSelectScrollLeft()
    {
        characterNumber--;
        if (characterNumber >= 0)
        {
            character = characterList[characterNumber];
        }
        else
        {
            characterNumber = 0;
        }
    }
    private void LevelSelectScrollLeft()
    {
        levelNumber--;
        if (levelNumber >= 0)
        {
            level.GetComponent<SpriteRenderer>().sprite = levelList[levelNumber];
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
            level.GetComponent<SpriteRenderer>().sprite = levelList[levelNumber];
        }
        else
        {
            levelNumber = levelList.Count - 1;
        }
    }
    private void TitleScreen()
    {
        titleScreen.SetActive(true);
        enterName.SetActive(false);
        characterSelect.SetActive(false);
        levelSelect.SetActive(false);
    }

    private void EnterNameScreen()
    {
        titleScreen.SetActive(false);
        enterName.SetActive(true);
        characterSelect.SetActive(false);
        levelSelect.SetActive(false);
    }

    private void CharacterSelectScreen()
    {
        if (playerName.text != "") { 
            titleScreen.SetActive(false);
            enterName.SetActive(false);
            characterSelect.SetActive(true);
            levelSelect.SetActive(false);
        }
    }
    private void LevelSelectScreen()
    {
        titleScreen.SetActive(false);
        enterName.SetActive(false);
        characterSelect.SetActive(false);
        levelSelect.SetActive(true);
    }
    private void StartGame()
    {

    }
}
