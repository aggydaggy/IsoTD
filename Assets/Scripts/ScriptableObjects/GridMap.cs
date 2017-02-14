using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridMap : ScriptableObject {
    public string grid;
    public Tower[] availableTowers;
    public EnemyWave[] EnemyWaves;
}
