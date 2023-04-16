using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    public void ResetScene()
    {
        // retrieve the index from the scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
