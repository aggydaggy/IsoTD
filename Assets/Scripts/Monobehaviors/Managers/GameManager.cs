﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapManager), typeof(SpawnManager), typeof(GameSettingsManager))]
[RequireComponent(typeof(TowerManager))]
public class GameManager : MonoBehaviour {
    
    public static GameManager Instance;

    //Set through code
    public MapManager mapManager { get; private set; }
    public SpawnManager spawnManager { get; private set; }
    public GameSettingsManager gameSettingsManager { get; private set; }
    public TowerManager towerManager { get; private set; }

    //Set through editor
    public GridMapDB MapsDB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            mapManager = GetComponent<MapManager>();
            spawnManager = GetComponent<SpawnManager>();
            gameSettingsManager = GetComponent<GameSettingsManager>();
            towerManager = GetComponent<TowerManager>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
