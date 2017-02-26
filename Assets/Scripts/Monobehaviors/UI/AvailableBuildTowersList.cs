using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableBuildTowersList : MonoBehaviour {
    
    public GameObject towerButtonPrefab;
    public Transform panel;

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.mapManager.currentMap.AvailableTowers.Length; i++)
        {
            GameObject towerButton = Instantiate(towerButtonPrefab);
            towerButton.transform.SetParent(panel, false);
            TowerBuildButton buildButton = towerButton.GetComponent<TowerBuildButton>();
            buildButton.InitializeButton(GameManager.Instance.mapManager.currentMap.AvailableTowers[i]);
        }
    }

    //public void OpenMenu(GameObject selectedTile)
    //{
        
    //    else
    //    {
    //        var children = GetComponentsInChildren<TowerBuildButton>();
    //        for(int i = 0; i < children.Length; i++)
    //        {
    //            children[i].InitializeButton(children[i].tower);
    //        }
    //    }
    //    gameObject.SetActive(true);
    //}

    //public void CloseMenu()
    //{
    //    gameObject.SetActive(false);
    //}
}
