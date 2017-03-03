using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public Enemy baseEnemyStats { get; private set; }
    public EnemyWave waveStats { get; private set; }
    public double currentHealth { get; private set; }
    public float speed { get; private set; }
    GameObject lastHitBy = null;

    public void SetInitialValues(EnemyWave waveValues, Enemy enemy)
    {
        baseEnemyStats = enemy;
        waveStats = waveValues;
        currentHealth = baseEnemyStats.BaseHealth + waveStats.HitPoints;
        speed = baseEnemyStats.BaseSpeed + waveStats.Speed;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0 && baseEnemyStats != null)
        {
            KillEnemy();
        }
	}

    public void TakeHit(Tower hitterInfo, GameObject hitter)
    {
        lastHitBy = hitter;
        currentHealth -= hitterInfo.BaseDamage;
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
        if (lastHitBy != null)
        {
            lastHitBy.GetComponent<TowerBehavior>().GotAKill();
            GameManager.Instance.mapManager.gold += baseEnemyStats.BaseGold + waveStats.MoneyPerKill;
            GameManager.Instance.spawnManager.currentlyExistingEnemies.Remove(gameObject);
        }
    }
}
