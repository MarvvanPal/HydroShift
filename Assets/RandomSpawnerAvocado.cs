using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnerAvocado : MonoBehaviour
{
    public float update;
    public GameObject cubePrefab;
    public int cube_counter = 0;

    void Awake() {
        update = 0.0f;
    }

    IEnumerator Start() {
    yield return new WaitForSeconds(0.8f);
    }

    // Update is called once per frame
    void Update() {
    
        update += Time.deltaTime;
        if (update > 0.1f){
            if (cube_counter < 50) {  
            Vector3 randomSpanPosition=new Vector3(Random.Range(-2,2),Random.Range(5,8), Random.Range(2,3));
            //Vector3 randomSpanPosition=new Vector3(0, 8, 2);
            Instantiate(cubePrefab,randomSpanPosition, Quaternion.identity);
            }
            update = 0.0f;
            cube_counter = cube_counter +1;
            }
    }
}
