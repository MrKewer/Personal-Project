using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable<float> hit = (IDamageable<float>)collision.gameObject.GetComponent(typeof(IDamageable<float>));
        if (hit != null)
        {
            hit.Damage(damageAmount);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
