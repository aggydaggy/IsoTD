using System.Collections;
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
        button.onClick.AddListener(Click);
    }

	// Update is called once per frame
	void Update () {
        button.interactable = cost <= GameManager.Instance.mapManager.gold;
	}


    public void Click()
    {
        if (button.interactable)
        {
            GameManager.Instance.towerManager.SetTowerToBuild(tower);
        }
    }
}
