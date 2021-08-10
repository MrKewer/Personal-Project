using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damageAmount = 200f; //The damage the explosion will do

    private void OnEnable() 
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 20); //Create a sphere
        foreach (Collider col in objectsInRange) //Find all the object in the sphere
        {
            if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Boss")) //Only find if its an Enemy or Boss then apply the damage
            {
                IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)col.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
                if (hit != null)
                {
                    hit.Damage(damageAmount, Enums.DamageType.Explosion, col.transform.position);
                }
            }
        }
        gameObject.SetActive(false);
    }
}


