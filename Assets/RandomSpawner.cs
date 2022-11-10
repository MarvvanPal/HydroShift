using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject cubePrefab;

    // Update is called once per frame
    void Update()
    {
        //Zählschleife für jeweiliges Produkt berücksichtigen. (Bsp. Kakao = 25.000 1L Würfel)
        //if(Input.GetKeyDown(KeyCode.Space)){
            Vector3 randomSpanPosition=new Vector3(Random.Range(-10,20),20, Random.Range(10,20));
            Instantiate(cubePrefab,randomSpanPosition, Quaternion.identity);
        //}
    }
}
