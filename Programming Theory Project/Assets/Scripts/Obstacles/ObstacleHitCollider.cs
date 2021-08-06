using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitCollider : Obstacles
{
    void Update() //Will override the parrent update
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point; //Get the position's point of the collision contact
        IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
        if (hit != null)
        {
            hit.Damage(damageAmount, Enums.DamageType.Collision, pos); //Does damage to whomever gets hit
            gameObject.transform.parent.gameObject.SetActive(false); //Sets this parents GameObject inactive
        }
    }
}
