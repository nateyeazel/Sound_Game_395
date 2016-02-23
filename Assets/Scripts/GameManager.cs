using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    private bool gameOver;
    private bool paused = false;
    private float difficulty;
	private int count;
	private int points;

    private Canvas MainMenu;
    private Canvas UI;
	private bool visibleObjects;
	public bool flicker;

	//UI Manager events
	public delegate void scoreChangedEvent(int newScore);
	public event scoreChangedEvent scoreChanged;
	public delegate void countChangedEvent(int newCount);
	public event countChangedEvent countChanged;
	public delegate void youLostEvent(string lossType);
	public event youLostEvent youLost;

	void Start () {
		GameObject player = GameObject.Find("Player");
		PlayerController pc = player.GetComponent<PlayerController>();


		visibleObjects = true;
		flicker = false;

		count = 0;
		points = 0;

	}

	void onItemCollected () {
		count += 1;
		countChanged.Invoke(count);
		//Update points here

	}

	void youLose () {
		gameOver = true;
		Time.timeScale = 0;
		youLost.Invoke("Fell off edge");
	}

	void collectedMovingTarget () {
		count += 1;
		countChanged.Invoke(count);
		//Update points here
		//Move target to next destination
	}

	void Update () {

		if(Time.timeSinceLevelLoad >= 30){
			GameObject target = GameObject.FindWithTag("Moving Target");
			if(visibleObjects){
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

			if(rb.position.y <= -10) {
				gameOver = true;
				Time.timeScale = 0;
			}

			if(gameOver && Input.GetKeyDown("r")){
				SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
				Time.timeScale = 1;
			}
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
    }
}
