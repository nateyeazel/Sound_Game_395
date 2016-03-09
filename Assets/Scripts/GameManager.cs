using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    private bool gameOver;
    //private float difficulty;
	private int count;
	private int score;

//    private Canvas MainMenu;
    private Canvas UI;
	private bool visibleObjects;
	public bool flicker;

	private GameObject player;
    private GameObject levelconfig;
    //UI Manager events
    public delegate void UIInitialize();
    public event UIInitialize UISetup = delegate { };
    public delegate void scoreChangedEvent(int newScore,int newCount);
	public event scoreChangedEvent scoreChanged = delegate { };
    public delegate void beaconsChangedEvent(int newnumBeacons);
    public event beaconsChangedEvent beaconsChanged = delegate { };
    public delegate void youLostEvent(string lossType);
	public event youLostEvent youLost = delegate { };
	public GameObject beacon;
	public Material darkSky;
    //
    private int numToWin;
    private bool restart = false;
    private int levelType;
    private int timeLimit;
    private int timetoBlindness;
    private int numBeacons;

	void Start () {
        Time.timeScale = 1;
        player = GameObject.Find("Player");
        levelconfig = GameObject.Find("LevelSettings");
        LevelSettings ls = levelconfig.GetComponent<LevelSettings>();
        numToWin = ls.numWin;
        levelType = ls.levelType;
        timeLimit = ls.timeLimit;
        timetoBlindness = ls.timeToBlindness;
        numBeacons = ls.numBeacons;
		if(UISetup != null){
        	UISetup.Invoke();
		}
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.hitEnemy += youLose;
        pc.fellOff += youLose;
        pc.itemCollected += onItemCollected;
        pc.collectedMovingTarget += collectedMovingTarget;
		pc.lightsOff += lightsOut;
        pc.beaconCollected += collectedBeacon;

        visibleObjects = true;
		flicker = false;
		count = 0;
		score = 0;
	}

	void onItemCollected (GameObject item) {
		count += 1;
        //Update points here
        float spawnTime = item.GetComponent<PickUp>().spawnTime;

        item.GetComponent<PickUp>().PickedUp();
        
        
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        scoreChanged.Invoke(score,count);
        //Update UI

    }


	void collectedMovingTarget (GameObject movingTarget) {
		count += 1;
        float spawnTime = movingTarget.GetComponent<PickUp>().spawnTime;
        movingTarget.GetComponent<PickUp>().PickedUp();
        //Move target to next destination
        score += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);
        movingTarget.GetComponent<PickUp>().spawnTime = Time.time;
        scoreChanged(score, count);
	}

    void collectedBeacon(GameObject beacon)
    {
        numBeacons += 1;
        beaconsChanged.Invoke(numBeacons);
        beacon.SetActive(false);
        

    }

	void lightsOut (GameObject item) {
		item.GetComponent<PickUp>().PickedUp();

		GameObject mainLight = GameObject.Find("Directional Light");
		mainLight.GetComponent<Light>().intensity = 0.1f; //Best way to make dark
//		RenderSettings.skybox.color = new Vector4(0,0,0,0);
		RenderSettings.skybox = darkSky;
        onItemCollected(item);
    }

    void youLose()
    {
        gameOver = true;
        Time.timeScale = 0;
        youLost.Invoke("Fell off edge");
        
    }

    void Update () {

		if(levelType == 1){
			if(Time.timeSinceLevelLoad >= timetoBlindness){
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
		}

        if (    restart)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }


        if (Input.GetKeyDown("b")){
            if (numBeacons > 0)
            {
                numBeacons -= 1;
                beaconsChanged.Invoke(numBeacons);
                Transform currentLocation = player.transform;
                Vector3 beaconLocation = player.transform.position + new Vector3(0, 1, 0);
				GameObject myBeacon =  Instantiate(beacon, beaconLocation, Quaternion.identity) as GameObject;
				myBeacon.GetComponent<AudioSource>().pitch += 0.5f * numBeacons;
            }
            else {
                //Display No beacons left message
                beaconsChanged.Invoke(numBeacons);
                
            }
		}
    }

    public void Restart()
    {
        restart = true;
    }
    
}
