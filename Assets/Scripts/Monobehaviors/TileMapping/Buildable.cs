using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TileInfo))]
public class Buildable : MonoBehaviour {

    public Color highlightColor;
    Color defaultColor;
    Renderer rend;
    TileInfo tileInfo;
    MapManager manager;
    GameObject previewTower;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        tileInfo = GetComponent<TileInfo>();
        defaultColor = rend.material.color;
        manager = FindObjectOfType<MapManager>();
        previewTower = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            rend.material.color = highlightColor;
            if (GameManager.Instance.towerManager.towerToBuild != null && !tileInfo.IsOccupied)
            {
                previewTower = Instantiate(GameManager.Instance.towerManager.towerToBuild.BaseTower, transform.position + (Vector3.up * tileInfo.BaseTileInfo.Y), Quaternion.Euler(0f, 0f, 0f));
                previewTower.GetComponent<SpriteRenderer>().material.color = new Color(25, 25, 25, 128);
                TowerBehavior towerBehavior = previewTower.GetComponent<TowerBehavior>();
                if (towerBehavior != null)
                {
                    Destroy(towerBehavior);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            rend.material.color = defaultColor;
            if (previewTower != null)
            {
                Destroy(previewTower);
                previewTower = null;
            }
        }
    }

    private void OnMouseDown()
    {
        if (tileInfo.Occupant == null && !EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.towerManager.TryToBuildTower(tileInfo);
        }
    }
}
