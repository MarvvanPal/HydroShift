using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandCollisionHandler : MonoBehaviour
{
    // the button that is about to be spawned
    public GameObject menuButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BoxHead")
        {
            menuButton.SetActive(true);
        }

        Debug.Log("Hand triggered with: " + other.gameObject.name);
        
    }

}
