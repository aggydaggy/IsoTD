using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyWave : ScriptableObject {

    public int Count;
    public float TimeBetweenSpawns;
    public int MoneyPerKill;
    public int ExpPerKill;
    public float Speed;
    public double HitPoints;
    public Enemy[] Enemies;
}
