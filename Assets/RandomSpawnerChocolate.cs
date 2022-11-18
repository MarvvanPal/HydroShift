using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnerChocolate : MonoBehaviour
{
    public float update;
    public GameObject cubePrefab;
    public int cube_counter = 0;

    void Awake() {
        update = 0.0f;
    }

    IEnumerator Start() {
    yield return new WaitForSeconds(0.2f);
    }

    // Update is called once per frame
    void Update() {
    
        update += Time.deltaTime;
        if (update > 0.01f){
            if (cube_counter < 170) {  
            Vector3 randomSpanPosition=new Vector3(Random.Range(-1,1),Random.Range(3,5), Random.Range(2,3));
            //Vector3 randomSpanPosition=new Vector3(0, 8, 2);
            Instantiate(cubePrefab,randomSpanPosition, Quaternion.identity);
            }
            update = 0.0f;
            cube_counter = cube_counter +1;
            }
    }
}
