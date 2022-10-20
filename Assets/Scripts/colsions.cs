using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colsions : MonoBehaviour
{
    public GameObject menue;
    public AudioSource audio;

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "cube2")
        {
            Debug.Log("Collision started.");
            //StartCoroutine(waiter());
            audio.Play();
            menue.SetActive(true);

        }
        /*IEnumerator waiter()
        {
            yield return new WaitForSeconds(2.0f);
        }*/

    }





}
