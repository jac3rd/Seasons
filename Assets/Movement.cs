using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Tilemap walkable, obstacles;
    void Start() {
        walkable = GameObject.Find("Walkable").GetComponent<Tilemap>();
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
