using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeat : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    public float speed = 10.0f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject levelList;
 
    void Start()
    {
        
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
        if (gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = levelList.GetComponent<LevelList>().GroundList[GameManager.Instance.levelSelectedNumber];
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = levelList.GetComponent<LevelList>().BackgroundList[GameManager.Instance.levelSelectedNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
