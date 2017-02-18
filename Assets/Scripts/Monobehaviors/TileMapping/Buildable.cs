using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileInfo))]
public class Buildable : MonoBehaviour {

    public Color highlightColor;
    Color defaultColor;
    Renderer rend;
    TileInfo tileInfo;
    GridManager manager;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        tileInfo = GetComponent<TileInfo>();
        defaultColor = rend.material.color;
        manager = FindObjectOfType<GridManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        rend.material.color = highlightColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (tileInfo.Occupant == null)
        {
            int randomIndex = Random.Range(0, manager.currentMap.AvailableTowers.Length);
            tileInfo.Occupant = Instantiate(manager.currentMap.AvailableTowers[randomIndex].BaseTower, transform.position + (Vector3.up * tileInfo.BaseTileInfo.Y), Quaternion.Euler(0f, 0f, 0f));
            tileInfo.IsOccupied = true;
            TowerBehavior towerBehavior = tileInfo.Occupant.GetComponent<TowerBehavior>();
            if (towerBehavior != null)
            {
                towerBehavior.SetInitialValues(manager.currentMap.AvailableTowers[randomIndex]);
            }
        }
        else
        {
            Debug.Log("THIS SOLIDER HAS " + tileInfo.Occupant.GetComponent<TowerBehavior>().kills + " KILLS");
        }
    }
}
