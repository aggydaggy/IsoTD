﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerBuildButton : MonoBehaviour {

    public Tower tower { get; private set; }
    public Button button;
    public Text nameText;
    public Text priceText;
    public int cost { get; private set; }

    public void InitializeButton(Tower towerToCreate)
    {
        tower = towerToCreate;
        button.image.overrideSprite = tower.TowerSelectPortrait;
        nameText.text = tower.Name; 
        priceText.text = tower.BaseCost.ToString() + "G";
        cost = tower.BaseCost;
        button.interactable = cost <= GameManager.Instance.mapManager.gold;
        //button.onClick.AddListener(Click);
    }

	// Update is called once per frame
	void Update () {
        button.interactable = cost <= GameManager.Instance.mapManager.gold;
	}



    public void Click()
    {
        if (button.interactable)
        {
            GameObject tile = GetComponentInParent<AvailableBuildTowersList>().selectedTile;
            TileInfo tileInfo = tile.GetComponent<TileInfo>();

            tileInfo.Occupant = Instantiate(tower.BaseTower, tile.transform.position + (Vector3.up * tileInfo.BaseTileInfo.Y), Quaternion.Euler(0f, 0f, 0f));
            tileInfo.IsOccupied = true;
            TowerBehavior towerBehavior = tileInfo.Occupant.GetComponent<TowerBehavior>();
            if (towerBehavior != null)
            {
                towerBehavior.SetInitialValues(tower);
            }
            GameManager.Instance.mapManager.gold -= cost;
            GetComponentInParent<AvailableBuildTowersList>().CloseMenu();
        }
    }
}
