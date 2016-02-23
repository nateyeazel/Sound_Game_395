using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
    public float spawnTime;
    float timeToCollect;
    public string type;
	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
        //initiate type (random moving, static, list moving)?
	}
	
	// Update is called once per frame
	void Update () {

	}
}
