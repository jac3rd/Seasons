using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WolfControl : MonoBehaviour
{
    public GameObject player;
    public Tilemap obstacles, wolves;
    public bool seenByWolf = false;
    public Vector3Int killerWolf;
    public Vector3Int killDirection;
    public TileBase killerWolfTile;
    public float wolfSpeed = 8f;
    public float wolfSpeedHelper = 1f;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        wolves = gameObject.GetComponent<Tilemap>();
        killerWolf = new Vector3Int();
        killDirection = new Vector3Int();
    }

    // Update is called once per frame
    void Update() {
        wolfSpeedHelper += wolfSpeed*Time.deltaTime;
        wolfSpeedHelper = Mathf.Min(wolfSpeedHelper, 1);
        if(seenByWolf) {
            if(wolves.WorldToCell(player.transform.position) == killerWolf) {
                //player.GetComponent<Movement>().eaten = true;
                seenByWolf = false;
            }
            else if(wolfSpeedHelper >= 1) {
                wolfSpeedHelper--;
                MoveWolf(killerWolf, killDirection);
            }
        }
    }

    void MoveWolf(Vector3Int killerWolf, Vector3Int killDirection) {
        wolves.SetTile(killerWolf+killDirection, killerWolfTile);
        //wolves.SetTile(killerWolf,null);
        killerWolf = killerWolf+killDirection;
    }

    public bool CheckWolves() {
        Vector3Int playerCoord = wolves.WorldToCell(player.transform.position);
        for(int x = wolves.cellBounds.xMin; x <= wolves.cellBounds.xMax; x++) {
            for(int y = wolves.cellBounds.yMin; y <= wolves.cellBounds.yMax; y++) {
                Vector3Int coord = new Vector3Int(x,y,0);
                TileBase wolf = wolves.GetTile(coord);
                Vector3Int direction = new Vector3Int();
                if(wolf != null) {
                    if(y == playerCoord.y) {
                        if(wolf.name == "Wolf_Left" && playerCoord.x <= x)
                            direction = Vector3Int.left;
                        else if(wolf.name == "Wolf_Right" && playerCoord.x >= x)
                            direction = Vector3Int.right;
                    }
                    else if(x == playerCoord.x) {
                        if(wolf.name == "Wolf_Down" && playerCoord.y <= y)
                            direction = Vector3Int.down;
                        else if(wolf.name == "Wolf_Up" && playerCoord.y >= y)
                            direction = Vector3Int.up;
                    }
                    if(direction.magnitude > 0) {
                        while(coord != playerCoord) {
                            TileBase t = obstacles.GetTile(coord);
                            if(t != null)
                                return false;
                            coord += direction;
                        }
                        seenByWolf = true;
                        killDirection = direction;
                        killerWolf = new Vector3Int(x,y,0);
                        killerWolfTile = wolves.GetTile(killerWolf);
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
