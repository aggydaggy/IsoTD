using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GameManager : MonoBehaviour {
    
    public static GameManager Instance;

    public GridManager gridManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            gridManager = GetComponent<GridManager>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
