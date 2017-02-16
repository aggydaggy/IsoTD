﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridMap : ScriptableObject {
    public string Grid;
    public string DecorationGrid;
    public int ScoreToUnlock;
    public Tower[] AvailableTowers;
    public EnemyWave[] EnemyWaves;
    public GridTile[] Tiles;
    public GridTile GroundTile;
    public Sprite[] Decorations;
}
