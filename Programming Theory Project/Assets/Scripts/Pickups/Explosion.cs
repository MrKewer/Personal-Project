using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damageAmount = 200f;

    private void OnEnable() 
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 20);
        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Boss"))
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


