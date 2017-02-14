using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridTile : ScriptableObject {

    public string Name;
    public int X;
    public int Y;
    public int Z;
    public GameObject Tile;
}
