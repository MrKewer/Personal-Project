using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] protected float damageAmount = 0;

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++) //check for all the coins and set them active
        {
            Transform child = transform.GetChild(i);
            if (child.tag == "Powerup")
            {
                child.gameObject.SetActive(true);
            }
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
