using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {

	public Tower towerToBuild { get; private set; }
    public TileInfo selectedTower { get; private set; }

    List<GameObject> towerRangeTiles = new List<GameObject>();

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
                towerBehavior.SetInitialValues(towerToBuild, tile.gameObject);
            }
            GameManager.Instance.mapManager.gold -= towerToBuild.BaseCost;
        }
    }

    public void ShowNewTowerRange(Transform position, float radius)
    {
        towerRangeTiles.Clear();
        Collider[] collidersInRange = Physics.OverlapSphere(position.position, radius);
        towerRangeTiles.AddRange(collidersInRange.Where(x => x.gameObject.GetComponent<Tilehighlight>() != null).Select(x => x.gameObject).ToList());
        for(int i = 0; i < towerRangeTiles.Count; i++)
        {
            towerRangeTiles[i].GetComponent<Tilehighlight>().currentHighlight = TileHighlightReason.TOWER_RANGE;
        }
    }

    public void StopShowingTowerRange()
    {
        for(int i = 0; i < towerRangeTiles.Count; i++)
        {
            towerRangeTiles[i].GetComponent<Tilehighlight>().currentHighlight = TileHighlightReason.NONE;
        }
        towerRangeTiles.Clear();
    }

    public void InitializeValues()
    {
        towerToBuild = null;
        selectedTower = null;
    }
}
