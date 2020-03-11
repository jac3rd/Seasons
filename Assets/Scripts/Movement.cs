using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public float actionsPerSecond = 4;
    public float actionsPerSecondHelper = 1;
    public SeasonsControl seasonsControl;
    public SceneTraveler sceneTraveler;
    public UIController uIController;
    public Tilemap walkable, obstacles;
    public GameObject winText,lossText;
    public Vector3 direction;
    public bool tryChangeSeason;
    public bool changingSeason = false;
    public bool slidingOnIce = false;
    public bool won = false;
    public bool starved = false;
    public float sceneChangeDelay = 3;
    public float timeUntilChange = 0;
    public int hunger = 3;
    void Start() {
        seasonsControl = GameObject.Find("Grid").GetComponent<SeasonsControl>();
        sceneTraveler = gameObject.GetComponent<SceneTraveler>();
        uIController = GameObject.Find("UI").GetComponent<UIController>();
        walkable = GameObject.Find("Walkable").GetComponent<Tilemap>();
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        winText = GameObject.Find("WinText");
        winText.SetActive(false);
        lossText = GameObject.Find("LossText");
        lossText.SetActive(false);
        Vector3Int coord = new Vector3Int();
        for(int x = walkable.cellBounds.xMin; x <= walkable.cellBounds.xMax; x++)
            for(int y = walkable.cellBounds.xMin; y <= walkable.cellBounds.yMax; y++) {
                coord.x = x; coord.y = y;
                TileBase t = walkable.GetTile(coord);
                if(t != null && t.name == "Start") {
                    transform.position = walkable.GetCellCenterLocal(coord);
                    goto endFindStart;
                }
            }
        endFindStart:;
    }

    // Update is called once per frame
    void Update() {
        actionsPerSecondHelper += actionsPerSecond * Time.deltaTime;
        if(actionsPerSecondHelper > 1)
            actionsPerSecondHelper = 1;
        tryChangeSeason = Input.GetButtonDown("Jump");
        if(won || starved) {
            timeUntilChange += Time.deltaTime;
            if(timeUntilChange >= sceneChangeDelay) {
                if(won)
                    sceneTraveler.goToNextScene();
                else
                    sceneTraveler.restartScene();
            }
        }
        else {
            if(!slidingOnIce)
                direction = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
            if(actionsPerSecondHelper >= 1) {
                if(changingSeason) {
                    actionsPerSecondHelper--;
                    changingSeason = false;
                    seasonsControl.ChangeSeason();
                }
                else if(direction.magnitude == 1 && !tryChangeSeason && checkTileAndMove(direction))
                    actionsPerSecondHelper--;
            }
            if(tryChangeSeason && hunger > 0) {
                seasonsControl.ChangeSeason();
                changingSeason = true;
                actionsPerSecondHelper--;
                hunger--;
                uIController.HungerUpdate();
                if(hunger <= 0) {
                    starved = true;
                    lossText.SetActive(true);
                }
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
            if(tileWalkable.name == "Ice") {
                slidingOnIce = true;
                return true;
            }
            else if(tileWalkable.name == "Finish") {
                won = true;
                winText.SetActive(true);
            }
            slidingOnIce = false;
            return true;
        }
        return false;
    }
}
