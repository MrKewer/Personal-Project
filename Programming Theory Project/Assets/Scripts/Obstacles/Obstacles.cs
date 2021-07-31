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
        if (transform.position.x < xDestroy) //When the position in the x direction reaches the point where the object can be disabled
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) {
        IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
        if (hit != null)
        {
            hit.Damage(damageAmount,"Collision", pos);
            gameObject.SetActive(false);
        }
    }
    }

}
