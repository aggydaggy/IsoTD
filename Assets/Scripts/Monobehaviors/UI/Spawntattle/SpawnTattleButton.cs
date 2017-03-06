using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnTattleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image EnemyImage;
    public GameObject StatsPanel;
    public Enemy Enemy;
    public EnemyWave Wave;

    public void InitializeData(Enemy enemy, EnemyWave wave)
    {
        Enemy = enemy;
        Wave = wave;
        StatsPanel.SetActive(false);
        StatsPanel.GetComponent<SpawnerTattle>().InitializeText(Enemy, Wave);
        EnemyImage.sprite = enemy.BaseEnemy.GetComponent<SpriteRenderer>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StatsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StatsPanel.SetActive(false);
    }
}
