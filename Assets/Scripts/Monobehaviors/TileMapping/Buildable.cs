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

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        tileInfo = GetComponent<TileInfo>();
        defaultColor = rend.material.color;
        manager = FindObjectOfType<MapManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            rend.material.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            rend.material.color = defaultColor;
        }
    }

    private void OnMouseDown()
    {
        if (tileInfo.Occupant == null && !EventSystem.current.IsPointerOverGameObject())
        {
            AvailableBuildTowersList[] panel = Resources.FindObjectsOfTypeAll<AvailableBuildTowersList>();
            if (panel.Length > 0)
            {
                panel[0].OpenMenu(gameObject);
            }
        }
    }
}
