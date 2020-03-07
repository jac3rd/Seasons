using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public float tilesPerSecond = 4f;
    public float tilesPerSecondHelper;
    public Tilemap walkable, obstacles;
    public Vector3 direction;
    void Start() {
        tilesPerSecondHelper = 1f;
        walkable = GameObject.Find("Walkable").GetComponent<Tilemap>();
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        for(int x = walkable.cellBounds.xMin; x <= walkable.cellBounds.xMax; x++) {
            for(int y = walkable.cellBounds.xMin; y <= walkable.cellBounds.yMax; y++) {
                Vector3Int coord = new Vector3Int(x,y,0);
                TileBase t = walkable.GetTile(coord);
                if(t != null && t.name == "Start") {
                    transform.position = walkable.GetCellCenterLocal(coord);
                    goto endFindStart;
                }
            }
        }
        endFindStart:;
    }

    // Update is called once per frame
    void Update() {
        tilesPerSecondHelper += tilesPerSecond*Time.deltaTime;
        tilesPerSecondHelper = Mathf.Min(tilesPerSecondHelper, 1);
        direction = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        if(tilesPerSecondHelper >= 1 && direction.magnitude == 1f && checkTileAndMove(direction)) {
            tilesPerSecondHelper -= 1;
        }
    }
    
    bool checkTileAndMove(Vector3 direction) {
        Vector3Int playerCoord = walkable.LocalToCell(transform.position);
        Vector3Int cellCoord = playerCoord + Vector3Int.RoundToInt(direction);
        TileBase tileWalkable = walkable.GetTile(cellCoord);
        TileBase tileObstacle = obstacles.GetTile(cellCoord);
        if(tileWalkable != null && tileObstacle == null) {
            transform.position = walkable.GetCellCenterLocal(cellCoord);
            if(tileWalkable.name == "Finish") {
                //Do something about winning
            }
            return true;
        }
        return false;
    }
}
