using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public Canvas UI;
    public Text winText;
    public Text countText;
	public Text scoreText;
    public Text countdownText;
    public GameObject pauseMenu;
    public GameObject lossMenu;
    public GameObject winMenu;
    private bool paused = false;
    private GameObject player;
    //Level Settings
    private int levelType;
    private int timeLimit;
    private int numToWin;
    private int numBeacons;
    private int timetoBlindness;
	private bool wonLevel;
    private bool lost;
    private bool noBeacons;
    private int count;
	private int score;
    
    // Use this for initialization
    void Start () {
        SetupUI();
		GameObject gameManagerObject = GameObject.Find("GameManager");
		GameManager gm = gameManagerObject.GetComponent<GameManager>();
        winText = GameObject.Find("InGameUI").GetComponent<UIManager>().winText;
        countdownText = GameObject.Find("InGameUI").GetComponent<UIManager>().countdownText;
        countText = GameObject.Find("InGameUI").GetComponent<UIManager>().countText;
        scoreText = GameObject.Find("InGameUI").GetComponent<UIManager>().scoreText;
        pauseMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().pauseMenu;
        lossMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().lossMenu;
        winMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().winMenu;
        gm.scoreChanged += updateScoreCount;
        gm.beaconsChanged += updateBeaconCount;
        gm.UISetup += SetupUI;
		gm.youLost += SetGameOver;
		wonLevel = false;
        winText.text = "";
		countdownText.text = "";
        countText.text = "";
        scoreText.text = ""; 

    }
	
    void SetupUI() {
        //Get Level Settings including type numToWin etc.
        GameObject levelconfig = GameObject.Find("LevelSettings");
        LevelSettings ls = levelconfig.GetComponent<LevelSettings>();
        levelType = ls.levelType;
        numToWin = ls.numWin;
        timeLimit = ls.timeLimit;
        timetoBlindness = ls.timeToBlindness;
        numBeacons = ls.numBeacons;

        return; }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
            if (paused)
            {
                paused = false;
                pauseMenu.gameObject.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.gameObject.SetActive(true);
                Cursor.visible = true;
                paused = true;

            }
        }



        if (wonLevel) {
            Cursor.visible = true;
            winMenu.SetActive(true);
            if (Input.GetKeyDown("m")){
				SceneManager.LoadScene("MainMenu");
				Time.timeScale = 1;
			}

			if(Input.GetKeyDown("r")){
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				
			}
		}
        if (!lost) { UpdateUI();}
        else
        {
            //Player Lost Show Lost Menu
            Time.timeScale = 0;
            lossMenu.SetActive(true);
            Cursor.visible = true;
        }
        
	}

    void UpdateUI()
    {
        if (levelType == 1)//1 - Collect X ammount going blind after Y time                                            
        {
            if (Time.timeSinceLevelLoad <= timetoBlindness)
            {
                countdownText.text = string.Format("Time until blindness: {0}", timetoBlindness - Time.timeSinceLevelLoad);
                if (Time.timeSinceLevelLoad <= 5)
                { //Display Objective
                    winText.text = string.Format("Collect {0} to Win", numToWin);

                }
                else if (timetoBlindness - Time.timeSinceLevelLoad < 10 & timetoBlindness - Time.timeSinceLevelLoad > 0)
                {
                    //if blind soon
                    winText.text = string.Format("Blind in: {0}", Mathf.RoundToInt(timetoBlindness - Time.timeSinceLevelLoad));
                }
                else {
                    winText.text = string.Format("");
                }
                //Case of Win
                if (count >= numToWin) { winText.text = "You Win!"; }
            }
            else
            {
                countdownText.text = string.Format("");
                winText.text = "";
            }
            SetCountText();
            SetScoreText();

            //Case of Win
            if (count >= numToWin) {
                wonLevel = true;
                Time.timeScale = 0.0f;
                //winText.text = "You Win! \n Press M to return to the main menu or R to retry"; 
            }


        }
        else if (levelType == 2)//2 - Avoid Enemies for X time
        {

            countdownText.text = string.Format("Time left: {0}", timeLimit - Time.timeSinceLevelLoad);
            //Case of Win
            if (Time.timeSinceLevelLoad <= 5)
            { //Display Objective
                winText.text = string.Format("Avoid Enemies for {0} Seconds to Win", timeLimit);

            }

            else if (timeLimit - Time.timeSinceLevelLoad < 5 && timeLimit - Time.timeSinceLevelLoad > 0)
            {
                winText.text = string.Format("Only {0} Seconds Left!", Mathf.RoundToInt(timeLimit - Time.timeSinceLevelLoad));
            }
            else
            {
                winText.text = "";
            }



            if (Time.timeSinceLevelLoad > timeLimit)
            {
                countdownText.text = string.Format("");
                winText.text = "";
                //winText.text = "You Win! \n Press M to return to the main menu or R to retry";
                Time.timeScale = 0.0f;
                wonLevel = true;
            }

        }
        else if (levelType == 3)//3- Traverse Maze collecting X pickups and Return to the Beginning
        {
            countdownText.text = string.Format("Time so far: {0}", Time.timeSinceLevelLoad);
            SetCountText();
            if (Time.timeSinceLevelLoad <= 5)
            { //Display Objective
                winText.text = string.Format("Collect {0} to Win", numToWin);
            } else if (count >= numToWin) {
                wonLevel = true;
                Time.timeScale = 0.0f;
            }
            else {
                countdownText.text = string.Format("Final Time: {0}", Time.timeSinceLevelLoad);
                winText.text = string.Format("");
            }
            scoreText.text = string.Format("Beacons: {0}", numBeacons);
            if (numBeacons==0 && Input.GetKey("b"))
            {

                winText.text = string.Format("No Beacons Left!");
            }


        }

    }

	void SetGameOver (string lossType){
        lost = true;
	}

	void SetCountText ()
	{
		countText.text = "Collected: " + count.ToString ();
    }


    
    void SetScoreText()
	{
        scoreText.text = "Score: " + score.ToString();
    }
    void updateScoreCount(int newScore,int newcount)
    {
       	score = newScore;
        count = newcount;
    }

    void updateBeaconCount(int newnumBeacons)
    {
        numBeacons = newnumBeacons;
        if(numBeacons == 0)
        {
            noBeacons = true;
            
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");

    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameObject pauseMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().pauseMenu;
        pauseMenu.SetActive(false);
    }
}
