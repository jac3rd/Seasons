using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIController : MonoBehaviour
{
    public Tilemap tilemap;
    public Movement movement;
    public int hunger;
    public TileBase barFull, barEmpty;
    // Start is called before the first frame update
    public Vector3Int startBar = new Vector3Int(-3,-5,0);
    public int barLength = 7;
    void Start() {
        tilemap = gameObject.GetComponent<Tilemap>();
        movement = GameObject.Find("Player").GetComponent<Movement>();
        HungerUpdate();
    }

    // Update is called once per frame
    public void HungerUpdate() {
        hunger = movement.hunger;
        int hungerTemp = hunger;
        for(Vector3Int segment = startBar; segment.x - startBar.x < barLength; segment.x++) {
            if(hungerTemp > 0)
                tilemap.SetTile(segment,barFull);
            else
                tilemap.SetTile(segment,barEmpty);
            hungerTemp--;
        }
    }
}
