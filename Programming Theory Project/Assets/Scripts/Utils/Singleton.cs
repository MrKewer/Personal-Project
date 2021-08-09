using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This helps eliminating multiple instances of a object
public class Singleton<T> //<> inside the brackets is the type(variable), T for generic
    : MonoBehaviour
    where T : Singleton<T> //Is required that the type must be an object that is ment to extend a singleton of that same type
{
    private static T instance;
    public static T Instance //Property
    {
        get { return instance; }
        //Set removed, dont want any external classes to set this property
    }

    public static bool IsInitialized //Is to check if the instance exist already or not
    {
        get { return instance != null; }
    }

    //Protected - accessable, virtual - can be overwritten
    protected virtual void Awake()
    {
        if (instance != null) //To see that there are not already an instance
        {
            //Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class.");
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this; //(T) To make sure of the type
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this) //To make sure that another instance can be created
        {
            instance = null;
        }
    }
}
