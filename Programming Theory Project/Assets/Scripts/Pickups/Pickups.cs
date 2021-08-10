using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public Enums.Pickups pickupType; //must be set in editor to know which pickup type the object is
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0); //Rotate the pickups
    }
}
