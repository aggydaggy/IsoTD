using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerTattle : MonoBehaviour {

    public Text speedText;
    public Text healthText;
    public Text amountText;
    public Text waveCounterText;

    private void Update()
    {
        InitializeText();
    }

    private void InitializeText()
    {
        int waveNum = GameManager.Instance.spawnManager.currentWave;
        if (waveNum < GameManager.Instance.mapManager.currentMap.EnemyWaves.Length)
        {
            EnemyWave waveInfo = GameManager.Instance.mapManager.currentMap.EnemyWaves[waveNum];
            Enemy enemy = waveInfo.Enemies[0]; //TODO: Handle multiple enemies in a wave
            speedText.text = "Speed: " + (waveInfo.Speed + enemy.BaseSpeed).ToString();
            healthText.text = "Health: " + (waveInfo.HitPoints + enemy.BaseHealth).ToString();
            amountText.text = "Amount: " + waveInfo.Count.ToString();
            waveCounterText.text = (waveNum + 1).ToString() + " of " + GameManager.Instance.mapManager.currentMap.EnemyWaves.Length.ToString();
        }
    }
}
