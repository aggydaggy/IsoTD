using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerTattle : MonoBehaviour {

    public Text speedText;
    public Text healthText;
    public Text amountText;
    public Text nameText;


    public void InitializeText(Enemy enemy, EnemyWave waveInfo)
    {
        speedText.text = "Speed: " + (waveInfo.Speed + enemy.BaseSpeed).ToString();
        healthText.text = "Health: " + (waveInfo.HitPoints + enemy.BaseHealth).ToString();
        amountText.text = "Amount: " + waveInfo.Count.ToString();
        nameText.text = enemy.Name;
    }
}
