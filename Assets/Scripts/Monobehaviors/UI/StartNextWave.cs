using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextWave : MonoBehaviour {

	public void StartWave()
    {
        GameManager.Instance.spawnManager.SpawnWave();
    }
}
