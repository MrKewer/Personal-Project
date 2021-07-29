using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scrip is to control the moving background and ground sprite
public class BackgroundRepeat : MonoBehaviour
{
    private Vector3 startPos; //To have the current start position of the sprite
    private float repeatWidth; //When the sprite will be repeated, calculated in the script
    [SerializeField] private GameObject levelList; //Gets the list that will be use to show the level selected
 
    void Start()
    {
        startPos = transform.position; //Gets the start position
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; //Calculates the half of the background to which it will repeat, and gives a nice flow
        if (gameObject.CompareTag("Ground"))
        {
            //Gets the selected level from GameManager and set the sprint in the level equal to it 
            gameObject.GetComponent<SpriteRenderer>().sprite = levelList.GetComponent<LevelList>().groundList[GameManager.Instance.levelSelectedNumber];
        }
        else
        {
            //Gets the selected level from GameManager and set the sprint in the level equal to it 
            gameObject.GetComponent<SpriteRenderer>().sprite = levelList.GetComponent<LevelList>().backgroundList[GameManager.Instance.levelSelectedNumber];
        }
    }

    void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime); // Moves the background on the x axis at the speed from GameManager
        if (transform.position.x < startPos.x - repeatWidth) // When the background reached the half of the sprite's width it would reset the position
        {
            transform.position = startPos;
        }
    }
}
