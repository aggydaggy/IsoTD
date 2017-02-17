using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public Enemy baseEnemyStats { get; private set; }
    [SerializeField]
    public double currentHealth { get; private set; }
    GameObject lastHitBy = null;

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
		if (currentHealth <= 0 && baseEnemyStats != null)
        {
            Destroy(gameObject);
            if (lastHitBy != null)
            {
                lastHitBy.GetComponent<TowerBehavior>().GotAKill();
            }
        }
	}

    public void TakeHit(Tower hitterInfo, GameObject hitter)
    {
        lastHitBy = hitter;
        currentHealth -= hitterInfo.BaseDamage;
    }
}
