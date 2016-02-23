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
	private int score;

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
        pc.hitEnemy += youLose;
        pc.itemCollected += onItemCollected;
        pc.collectedMovingTarget += collectedMovingTarget;
        visibleObjects = true;
		flicker = false;
		count = 0;
		score = 0;
	}

	void onItemCollected (GameObject item) {
		count += 1;
        Debug.Log(count);
        countChanged.Invoke(count);
        //Update points here
        float spawnTime = item.GetComponent<PickUp>().spawnTime;
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        scoreChanged.Invoke(score);
    }

	void youLose () {
		gameOver = true;
        Time.timeScale = 0;
		youLost.Invoke("Fell off edge");
	}

	void collectedMovingTarget (GameObject movingTarget) {
		count += 1;
        float spawnTime = movingTarget.GetComponent<PickUp>().spawnTime;
        //Update Location
        float new_x = UnityEngine.Random.Range(-10f, 10f);
        float new_z = UnityEngine.Random.Range(-10f, 10f);
        movingTarget.transform.position = new Vector3(new_x, 0.5f, new_z);
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        movingTarget.GetComponent<PickUp>().spawnTime = Time.time;
        scoreChanged.Invoke(score);
		//Move target to next destination
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
    }
}
