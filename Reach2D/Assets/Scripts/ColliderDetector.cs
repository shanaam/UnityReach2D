using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour {

    //public bool targetHit = false;

    public ExampleController exampleController;

    public GameObject homePosition;

    private void OnTriggerEnter(Collider other)
    {
        //there should be an option for home too
        if (other.CompareTag("Home"))
        {
            exampleController.EndAndPrepare();
        }
        else if (other.CompareTag("Target"))
        {
            //Destroy old target
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

            for (var i = 0; i < targets.Length; i++)
            {
                Destroy(targets[i]);
            }
            
            //Bring up the home position
            homePosition.SetActive(true);
        }
    }
}
