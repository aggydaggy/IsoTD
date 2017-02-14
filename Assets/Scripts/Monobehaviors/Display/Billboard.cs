using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    private void Awake()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    private void OnRenderObject()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
