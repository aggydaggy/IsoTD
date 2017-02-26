using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour {

    public bool IsOccupied
    {
        get
        {
            return Occupant != null;
        }
    }
    public GameObject Occupant;
    public bool IsBuildable;
    public bool IsWalkable;
    public Dictionary<Vector2, int> WeightToGoal = new Dictionary<Vector2, int>();
    public List<Vector2> GoalsCheckedFor;
    public Vector2 GridPosition;
    public GridTile BaseTileInfo;
    public List<GameObject> Grounds;
    public int ElevationLevels;
}
