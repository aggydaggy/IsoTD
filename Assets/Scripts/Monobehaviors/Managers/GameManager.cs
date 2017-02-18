using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GameManager : MonoBehaviour {

	public int SelectedLevel = 0;

    GridManager gridManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        gridManager = GetComponent<GridManager>();
    }

}
