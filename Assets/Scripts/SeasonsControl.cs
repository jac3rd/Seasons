﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeasonsControl : MonoBehaviour
{
    private string[] seasons = {"Summer","Autumn","Winter","Spring"};
    private int seasonID;
    public string season;
    public Tilemap walkable, obstacles;
    public TileBase water, ice, grassSummer, grassAutumn, grassWinter, grassSpring, wood, fire;
    // Start is called before the first frame update
    void Start() {
        seasonID = 0;
        season = seasons[seasonID];
        walkable = GameObject.Find("Walkable").GetComponent<Tilemap>();
        obstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    public void ChangeSeason() {
        seasonID++;
        if(seasonID >= seasons.Length)
            seasonID = 0;
        season = seasons[seasonID];
        int xMin = Mathf.Min(walkable.cellBounds.xMin,obstacles.cellBounds.xMin);
        int xMax = Mathf.Max(walkable.cellBounds.xMax,obstacles.cellBounds.xMax);
        int yMin = Mathf.Min(walkable.cellBounds.yMin,obstacles.cellBounds.yMin);
        int yMax = Mathf.Max(walkable.cellBounds.yMax,obstacles.cellBounds.yMax);
        Vector3Int coord = new Vector3Int();
        for(int x = xMin; x <= xMax; x++)
            for(int y = yMin; y <= yMax; y++) {
                coord.x = x; coord.y = y;
                TileBase tileWalkable = walkable.GetTile(coord);
                TileBase tileObstacle = obstacles.GetTile(coord);
                switch(seasonID) {
                case 0:
                    if(tileWalkable != null) {
                        if(tileWalkable.name == "Grass_Spring")
                            walkable.SetTile(coord,grassSummer);
                        else if(tileWalkable.name == "Wood") {
                            walkable.SetTile(coord,null);
                            obstacles.SetTile(coord,fire);
                        }
                    }
                    break;
                case 1:
                    if(tileWalkable != null)
                        if(tileWalkable.name == "Grass_Summer")
                            walkable.SetTile(coord,grassAutumn);
                    if(tileObstacle != null)
                        if(tileObstacle.name == "Fire") {
                            obstacles.SetTile(coord,null);
                            walkable.SetTile(coord,wood);
                        }
                    break;
                case 2:
                    if(tileObstacle != null && tileObstacle.name == "Water") {
                        obstacles.SetTile(coord, null);
                        walkable.SetTile(coord,ice);
                    }
                    if(tileWalkable != null && tileWalkable.name == "Grass_Autumn")
                        walkable.SetTile(coord,grassWinter);
                    break;
                default:
                    if(tileWalkable != null) {
                        if(tileWalkable.name == "Ice") {
                            obstacles.SetTile(coord,water);
                            walkable.SetTile(coord,null);
                        }
                        if(tileWalkable.name == "Grass_Winter") {
                            walkable.SetTile(coord,grassSpring);
                        }
                    }
                    break;
                }
            }
    }
}
