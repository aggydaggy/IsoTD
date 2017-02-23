using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableBuildTowersList : MonoBehaviour {

    public GameObject selectedTile { get; private set; }
    public GameObject towerButtonPrefab;
    public Transform panel;
    bool built = false;

    private void Start()
    {
        
    }

    public void OpenMenu(GameObject selectedTile)
    {
        this.selectedTile = selectedTile;
        if (!built)
        {
            for (int i = 0; i < GameManager.Instance.mapManager.currentMap.AvailableTowers.Length; i++)
            {
                GameObject towerButton = Instantiate(towerButtonPrefab);
                towerButton.transform.SetParent(panel, false);
                TowerBuildButton buildButton = towerButton.GetComponent<TowerBuildButton>();
                buildButton.InitializeButton(GameManager.Instance.mapManager.currentMap.AvailableTowers[i]);
            }
            built = true;
        }
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
