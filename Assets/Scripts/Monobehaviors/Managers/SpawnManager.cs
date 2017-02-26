using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int currentWave { get; private set; }
    public int currentlyRunningWaves { get; private set; }
    private List<EnemyWave> wavesCurrentlyRunning = new List<EnemyWave>();

    public void InitiateSpawner()
    {
        currentWave = 0;
        currentlyRunningWaves = 0;
        wavesCurrentlyRunning.Clear();
    }

    private void Update()
    {
        currentlyRunningWaves = wavesCurrentlyRunning.Count;
        //DEBUG
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        if (currentWave < GameManager.Instance.mapManager.currentMap.EnemyWaves.Length)
        {
            EnemyWave waveInfo = GameManager.Instance.mapManager.currentMap.EnemyWaves[currentWave];
            StartCoroutine(RunWave(waveInfo));
            currentWave++;
        }
    }

    IEnumerator RunWave(EnemyWave wave)
    {
        wavesCurrentlyRunning.Add(wave);
        WaitForSeconds spawnWait = new WaitForSeconds(wave.TimeBetweenSpawns);
        int spawnCount = wave.Count;
        for (int i = spawnCount; i >= 0; i--)
        {
            for (int j = 0; j < wave.Enemies.Length; j++)
            {
                Vector2 spawnTile = GameManager.Instance.mapManager.StartTiles[Random.Range(0, GameManager.Instance.mapManager.StartTiles.Count)];
                GameObject spawnTileObject = GameManager.Instance.mapManager.GetTile((int)spawnTile.x, (int)spawnTile.y);
                GameObject enemySpawned = Instantiate(wave.Enemies[j].BaseEnemy, spawnTileObject.transform.position + (Vector3.up * 2), Quaternion.Euler(0f, 0f, 0f));
                WalkToGoal enemyWalk = enemySpawned.GetComponent<WalkToGoal>();
                enemyWalk.SetValues(wave, spawnTile);
                enemySpawned.GetComponent<EnemyBehavior>().SetInitialValues(wave, wave.Enemies[j]);
            }
            yield return spawnWait;
        }
        wavesCurrentlyRunning.Remove(wave);
    }
}
