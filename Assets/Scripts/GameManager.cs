﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    private bool gameOver;
    private bool paused = false;
    private float difficulty;
	private int count;
	private int score;

    private Canvas MainMenu;
    private Canvas UI;
	private bool visibleObjects;
	public bool flicker;

	private GameObject player;

	//UI Manager events
	public delegate void scoreChangedEvent(int newScore);
	public event scoreChangedEvent scoreChanged;
	public delegate void countChangedEvent(int newCount);
	public event countChangedEvent countChanged;
	public delegate void youLostEvent(string lossType);
	public event youLostEvent youLost;
	public GameObject beacon;
	public Material darkSky;

	void Start () {
		player = GameObject.Find("Player");
		PlayerController pc = player.GetComponent<PlayerController>();
        pc.hitEnemy += youLose;
        pc.fellOff += youLose;
        pc.itemCollected += onItemCollected;
        pc.collectedMovingTarget += collectedMovingTarget;
		pc.lightsOff += lightsOut;

        visibleObjects = true;
		flicker = false;
		count = 0;
		score = 0;
	}

	void onItemCollected (GameObject item) {
		count += 1;
        item.GetComponent<PickUp>().PickedUp();
        //Update points here
        float spawnTime = item.GetComponent<PickUp>().spawnTime;
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        scoreChanged.Invoke(score);
    }


	void collectedMovingTarget (GameObject movingTarget) {
		count += 1;
        float spawnTime = movingTarget.GetComponent<PickUp>().spawnTime;
        movingTarget.GetComponent<PickUp>().PickedUp();
        //Move target to next destination
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        movingTarget.GetComponent<PickUp>().spawnTime = Time.time;
        scoreChanged.Invoke(score);

	}

	void lightsOut (GameObject item) {
		GameObject mainLight = GameObject.Find("Directional Light");
		mainLight.GetComponent<Light>().intensity = 0.1f; //Best way to make dark
//		RenderSettings.skybox.color = new Vector4(0,0,0,0);
//		RenderSettings.skybox = darkSky;
        onItemCollected(item);
    }

    void youLose()
    {
        gameOver = true;
        Time.timeScale = 0;
        youLost.Invoke("Fell off edge");
    }

    void Update () {

		if(Time.timeSinceLevelLoad >= 30){
            GameObject target = GameObject.FindWithTag("Moving Target");
            if (visibleObjects){
				visibleObjects = false;
				target.GetComponent<Renderer>().enabled = false;
				target.transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
			}

			if (flicker)
			{
				if ((int)Time.realtimeSinceStartup % 5 == 0)
				{
					target.GetComponent<Renderer>().enabled = true;
				}
				else if ((int)Time.realtimeSinceStartup % 5 == 1)
				{
					target.GetComponent<Renderer>().enabled = false;
				}
			}
		}

        if (gameOver && Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown("p"))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
            }
        }
		if (Input.GetKeyDown("b")){
			Transform currentLocation = player.transform;
			Vector3 beaconLocation = player.transform.position + new Vector3 (0, 1, 0);
			Instantiate(beacon, beaconLocation, Quaternion.identity);
		}
    }
}
