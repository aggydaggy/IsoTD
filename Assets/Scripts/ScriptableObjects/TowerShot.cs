using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerShot : ScriptableObject {

    public bool DoesAoe;
    public float BaseAoeRadius;
    public float BaseTimeBetweenShots;
    public double BaseDamage;
    public Color ShotColor;
}
