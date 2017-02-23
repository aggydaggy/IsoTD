using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUpdate : MonoBehaviour {

    public Text goldText;
    public Text livesText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        livesText.text = GameManager.Instance.mapManager.lives.ToString();
        goldText.text = GameManager.Instance.mapManager.gold.ToString() + "G";
    }
}
