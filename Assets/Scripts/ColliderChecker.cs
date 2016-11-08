using UnityEngine;
using System.Collections;

public class ColliderChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("FUCKING START ALREADY");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggeeEnter(Collider other)
    {
        Debug.Log("COLLIDES WITH SOMETHING!");
    }

    /*
    void OnTriggerStay(Collider other)
    {
        Debug.Log("COLLIDES WITH SOMETHING!");
    }
    */
}