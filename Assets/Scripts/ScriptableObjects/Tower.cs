﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tower : ScriptableObject {

    public string Name;
    public int BaseCost;
    public int BaseRadius;
    public GameObject BaseTower;
    public GameObject BaseBullet;
    public Vector3 BulletSpawnOffset;
    public Sprite TowerSelectPortrait;
    public float BaseBulletSpeed;
    public bool TargetsRadius;
    public bool DoesShotAoeSpread;
    public float BaseAoeSpreadRadius;
    public double BaseAoeDamagePercentage;
    public float BaseTimeBetweenShots;
    public double BaseDamage;
    public Color ShotColor;
}
