using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTattleEnemies : MonoBehaviour {

    public GameObject EnemyButtonPrefab;
    public Transform Panel;

    List<GameObject> EnemyWaveButtons = new List<GameObject>();


	
	// Update is called once per frame
	void Start () {
        EnemyWaveButtons.Clear();
        InitializeData();
	}

    private void OnDisable()
    {
        for(int i = 0; i < EnemyWaveButtons.Count; i++)
        {
            Destroy(EnemyWaveButtons[i]);
        }
    }

    private void OnEnable()
    {
        EnemyWaveButtons.Clear();
        InitializeData();
    }

    void InitializeData()
    {
        int waveNum = GameManager.Instance.spawnManager.currentWave;
        if (waveNum < GameManager.Instance.mapManager.currentMap.EnemyWaves.Length)
        {
            EnemyWave waveInfo = GameManager.Instance.mapManager.currentMap.EnemyWaves[waveNum];
            List<Enemy> enemy = waveInfo.Enemies.ToList(); //TODO: Handle multiple enemies in a wave
            for(int i = 0; i < enemy.Count; i++)
            {
                GameObject button = Instantiate(EnemyButtonPrefab);
                button.transform.SetParent(Panel, false);
                SpawnTattleButton buttonInfo = button.GetComponent<SpawnTattleButton>();
                buttonInfo.InitializeData(enemy[i], waveInfo);
                EnemyWaveButtons.Add(button);
            }
        }
    }
}
