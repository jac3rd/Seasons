using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public float actionsPerSecond = 4f;
    public float actionsPerSecondHelper;
    public SeasonsControl seasonsControl;
    public Tilemap walkable, obstacles;
    public Vector3 direction;
    public bool tryChangeSeason;
    void Start() {
        actionsPerSecondHelper = 1f;
        seasonsControl = GameObject.Find("Grid").GetComponent<SeasonsControl>();
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
        actionsPerSecondHelper += actionsPerSecond*Time.deltaTime;
        actionsPerSecondHelper = Mathf.Min(actionsPerSecondHelper, 1);
        direction = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        tryChangeSeason = Input.GetButtonDown("Jump");
        if(actionsPerSecondHelper >= 1) {
            if(direction.magnitude == 1f && !tryChangeSeason && checkTileAndMove(direction)) {
                actionsPerSecondHelper -= 1;
            }
            else if(direction.magnitude != 1f && tryChangeSeason) {
                seasonsControl.ChangeSeason();
                actionsPerSecondHelper -= 1;
            }
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
