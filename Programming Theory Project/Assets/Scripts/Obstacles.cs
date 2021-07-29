using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float xDestroy = -35f; //The x position for when the object will be disabled

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);//Move the object on the x position, times the speed
        if (transform.position.x < xDestroy) //When the position in the x direction reaches the point where the object can be disabled
        {
            gameObject.SetActive(false);
        }
    }
    
}
