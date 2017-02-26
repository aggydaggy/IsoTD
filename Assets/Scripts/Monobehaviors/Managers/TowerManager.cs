using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {

	public Tower towerToBuild { get; private set; }
    public TileInfo selectedTower { get; private set; }

    public void SetTowerToBuild(Tower tower)
    {
        towerToBuild = tower;
    }

    public void SelectTower(TileInfo tile)
    {
        selectedTower = tile;
    }

    public void TryToBuildTower(TileInfo tile)
    {
        if(towerToBuild != null && towerToBuild.BaseCost <= GameManager.Instance.mapManager.gold && !tile.IsOccupied)
        {
            tile.Occupant = Instantiate(towerToBuild.BaseTower, tile.gameObject.transform.position + (Vector3.up * tile.BaseTileInfo.Y), Quaternion.Euler(0f, 0f, 0f));
            TowerBehavior towerBehavior = tile.Occupant.GetComponent<TowerBehavior>();
            if (towerBehavior != null)
            {
                towerBehavior.SetInitialValues(towerToBuild);
            }
            GameManager.Instance.mapManager.gold -= towerToBuild.BaseCost;
        }
    }

    public void InitializeValues()
    {
        towerToBuild = null;
        selectedTower = null;
    }
}
