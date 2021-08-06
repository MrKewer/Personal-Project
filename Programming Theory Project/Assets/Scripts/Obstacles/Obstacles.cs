using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float xDestroy = -35f; //The x position for when the object will be disabled
    [SerializeField] protected float damageAmount = 10f;
    void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);//Move the object on the x position, times the speed
        if (transform.position.x < xDestroy || transform.position.x > -xDestroy) //When the position in the x direction reaches the point where the object can be disabled
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point; //Get the position's point of the collision contact
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) //Does damage only if its the enemy or boss, Lets the player jump and run on the obstacles
        { 
            IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
            if (hit != null)
            {
                hit.Damage(damageAmount, Enums.DamageType.Collision, pos);
                gameObject.SetActive(false);
            }
        }
    }

}
