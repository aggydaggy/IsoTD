using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int currentWave { get; private set; }
    public List<GameObject> currentlyExistingEnemies = new List<GameObject>();
    public bool currentlySpawningEnemies { get; private set; }

    public void InitiateSpawner()
    {
        currentWave = 0;
        currentlyExistingEnemies = new List<GameObject>();
        currentlySpawningEnemies = false;
    }

    private void Update()
    {
        if(currentlyExistingEnemies.Count == 0 && !currentlySpawningEnemies)
        {
            Time.timeScale = 1;
        }
    }

    public void SpawnWave()
    {
        if (currentWave < GameManager.Instance.mapManager.currentMap.EnemyWaves.Length)
        {
            GameManager.Instance.towerManager.SetTowerToBuild(null);
            currentlySpawningEnemies = true;
            EnemyWave waveInfo = GameManager.Instance.mapManager.currentMap.EnemyWaves[currentWave];
            StartCoroutine(RunWave(waveInfo));
            currentWave++;
        }
    }

    IEnumerator RunWave(EnemyWave wave)
    {
        WaitForSeconds spawnWait = new WaitForSeconds(wave.TimeBetweenSpawns);
        int spawnCount = wave.Count;
        for (int i = spawnCount; i > 0; i--)
        {
            for (int j = 0; j < wave.Enemies.Length; j++)
            {
                Vector2 spawnTile = GameManager.Instance.mapManager.StartTiles[Random.Range(0, GameManager.Instance.mapManager.StartTiles.Count)];
                GameObject spawnTileObject = GameManager.Instance.mapManager.GetTile((int)spawnTile.x, (int)spawnTile.y);
                GameObject enemySpawned = Instantiate(wave.Enemies[j].BaseEnemy, spawnTileObject.transform.position + (Vector3.up * 2), Quaternion.Euler(0f, 0f, 0f));
                EnemyBehavior enemyInfo = enemySpawned.GetComponent<EnemyBehavior>();
                enemyInfo.SetInitialValues(wave, wave.Enemies[j]);
                WalkToGoal enemyWalk = enemySpawned.GetComponent<WalkToGoal>();
                enemyWalk.SetValues(enemyInfo, spawnTile);
                currentlyExistingEnemies.Add(enemySpawned);
                yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
            }
            yield return spawnWait;
        }
        currentlySpawningEnemies = false;
    }
}
