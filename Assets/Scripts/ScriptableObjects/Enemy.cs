using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject {

    public string Name;
    public string BaseHealth;
    public string BaseExp;
    public string BaseGold;
    public string BaseSpeed;
    public GameObject BaseEnemy;
}
