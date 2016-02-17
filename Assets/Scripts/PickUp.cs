using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
    float spawnTime;
    float timeToCollect;
    public Canvas UI; 
	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        timeToCollect = spawnTime - Time.time;
            
	}
}
