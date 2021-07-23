using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float xDestroy = -35f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);
        if (transform.position.x < xDestroy)
        {
            gameObject.SetActive(false);
        }
    }
    
}
