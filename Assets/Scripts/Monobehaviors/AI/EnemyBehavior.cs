using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public Enemy baseEnemyStats { get; private set; }
    public double currentHealth { get; private set; }

    public void SetInitialValues(Enemy enemy)
    {
        baseEnemyStats = enemy;
        currentHealth = baseEnemyStats.BaseHealth;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
