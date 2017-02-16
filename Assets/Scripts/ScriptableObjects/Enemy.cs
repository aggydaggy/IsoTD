using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject {

    public string Name;
    public double BaseHealth;
    public double BaseExp;
    public int BaseGold;
    public float BaseSpeed;
    public GameObject BaseEnemy;
}
