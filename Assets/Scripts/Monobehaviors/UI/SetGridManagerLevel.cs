using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGridManagerLevel : MonoBehaviour {

	public void SetLoadLevel(int level)
    {
        GameManager.Instance.mapManager.SetMapToLoad(level);
    }
}
