using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileHighlightReason
{
    NONE,
    TOWER_RANGE,
    UPGRADE_TOWER_RANGE
}

public class Tilehighlight : MonoBehaviour {

    public Color TowerRange;
    public Color UpgradeTowerRange;
    public TileHighlightReason currentHighlight;

    Renderer tileRenderer;
    Color originalColor;

	// Use this for initialization
	void Start () {
        tileRenderer = gameObject.GetComponent<Renderer>();
        originalColor = tileRenderer.material.color;
        currentHighlight = TileHighlightReason.NONE;
	}
	
	// Update is called once per frame
	void Update () {
            switch(currentHighlight)
            {
                case TileHighlightReason.NONE:
                    tileRenderer.material.color = originalColor;
                    break;
                case TileHighlightReason.TOWER_RANGE:
                    tileRenderer.material.color = Color.Lerp(originalColor, TowerRange, Mathf.PingPong(Time.time, .5f));
                    break;
                case TileHighlightReason.UPGRADE_TOWER_RANGE:
                    tileRenderer.material.color = Color.Lerp(originalColor, UpgradeTowerRange, Mathf.PingPong(Time.time, .5f));
                    break;
            }
	}
}
