using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTraveler : MonoBehaviour
{
    public int prevSceneID, nextSceneID, currSceneID;
    public Scene prevScene, nextScene, currScene;
    // Start is called before the first frame update
    void Start() {
        currScene = SceneManager.GetActiveScene();
        currSceneID = currScene.buildIndex;
        prevSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        nextSceneID = SceneManager.GetActiveScene().buildIndex + 1;
        if(prevSceneID >= 0)
            prevScene = SceneManager.GetSceneByBuildIndex(prevSceneID);
        if(nextSceneID < SceneManager.sceneCountInBuildSettings)
            nextScene = SceneManager.GetSceneByBuildIndex(nextSceneID);
    }

    // Update is called once per frame
    public void goToNextScene() {
        SceneManager.LoadScene(nextSceneID);
    }
}
