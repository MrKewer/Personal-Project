using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float xDestroy = -35f; //The x position for when the object will be disabled
    [SerializeField] private float damageAmount = 10f;
    void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);//Move the object on the x position, times the speed
        if (transform.position.x < xDestroy) //When the position in the x direction reaches the point where the object can be disabled
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
        IDamageable<float> hit = (IDamageable<float>)collision.gameObject.GetComponent(typeof(IDamageable<float>));
        if (hit != null)
        {
            hit.Damage(damageAmount);
            gameObject.SetActive(false);
        }
    }
    }

}
