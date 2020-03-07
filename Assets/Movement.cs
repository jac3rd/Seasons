using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public float actionsPerSecond = 4f;
    public float actionsPerSecondHelper;
    public SeasonsControl seasonsControl;
    public SceneTraveler sceneTraveler;
    public UIController uIController;
    public Tilemap walkable, obstacles;
    public GameObject winText,lossText;
    public Vector3 direction;
    public bool tryChangeSeason;
    public bool won = false;
    public bool lost = false;
    public float sceneChangeDelay = 3f;
    public float timeUntilChange = 0f;
    public int hunger = 3;
    void Start() {
        actionsPerSecondHelper = 1f;
        seasonsControl = GameObject.Find("Grid").GetComponent<SeasonsControl>();
        sceneTraveler = gameObject.GetComponent<SceneTraveler>();
        uIController = GameObject.Find("UI").GetComponent<UIController>();
        walkable = GameObject.Find("Walkable").GetComponent<Tilemap>();
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        winText = GameObject.Find("WinText");
        winText.SetActive(false);
        lossText = GameObject.Find("LossText");
        lossText.SetActive(false);
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
        if(won || lost) {
            timeUntilChange += Time.deltaTime;
            if(timeUntilChange >= sceneChangeDelay) {
                if(won) {
                    sceneTraveler.goToNextScene();
                }
                else {
                    sceneTraveler.restartScene();
                }
            }
        }
        else {
            actionsPerSecondHelper += actionsPerSecond*Time.deltaTime;
            actionsPerSecondHelper = Mathf.Min(actionsPerSecondHelper, 1);
            direction = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
            tryChangeSeason = Input.GetButtonDown("Jump");
            if(actionsPerSecondHelper >= 1) {
                if(direction.magnitude == 1f && !tryChangeSeason && checkTileAndMove(direction)) {
                    actionsPerSecondHelper -= 1;
                }
            }
            if(tryChangeSeason && hunger > 0) {
                seasonsControl.ChangeSeason();
                actionsPerSecondHelper -= 1;
                hunger--;
                uIController.HungerUpdate();
                if(hunger <= 0) {
                    lost = true;
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
            if(tileWalkable.name == "Finish") {
                won = true;
                winText.SetActive(true);
            }
            return true;
        }
        return false;
    }
}
