using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUISwap : MonoBehaviour {

    public GameObject[] UIPresentWhenWaveRuns;
    public GameObject[] UIPresentWhenWaveStops;
    private bool lastFrameRunningState = false;
    private bool currentFrameRunningState = false;

	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.spawnManager.currentlyExistingEnemies.Count > 0 || GameManager.Instance.spawnManager.currentlySpawningEnemies)
        {
            currentFrameRunningState = true;
        }
        else
        {
            currentFrameRunningState = false;
        }

        if (currentFrameRunningState != lastFrameRunningState)
        {
            if (currentFrameRunningState == true)
            {
                for (int i = 0; i < UIPresentWhenWaveRuns.Length; i++)
                {
                    UIPresentWhenWaveRuns[i].SetActive(true);
                }
                for (int i = 0; i < UIPresentWhenWaveStops.Length; i++)
                {
                    UIPresentWhenWaveStops[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < UIPresentWhenWaveStops.Length; i++)
                {
                    UIPresentWhenWaveStops[i].SetActive(true);
                }
                for (int i = 0; i < UIPresentWhenWaveRuns.Length; i++)
                {
                    UIPresentWhenWaveRuns[i].SetActive(false);
                }
            }
        }
        lastFrameRunningState = currentFrameRunningState;
	}
}
