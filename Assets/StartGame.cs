using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public SceneTraveler sceneTraveler;
    // Start is called before the first frame update
    void Start() {
        sceneTraveler = gameObject.GetComponent<SceneTraveler>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Jump")) {
            sceneTraveler.goToNextScene();
        }
    }
}
