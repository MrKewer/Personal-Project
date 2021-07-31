using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitToMain);
    }

    void ExitToMain()
    {
        GameManager.Instance.ExitToMain();
    }
}
