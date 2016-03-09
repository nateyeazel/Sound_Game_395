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
	public GameObject inputName;
    public GameObject inputName2;
    public GameObject highscoreMenu;
    public GameObject surviveLossMenu;
    private bool paused = false;
    private GameObject player;
    //Level Settings
	private string levelname;
    private int levelType;
    private int timeLimit;
    private int numToWin;
    private int numBeacons;
    private int timetoBlindness;
	private bool wonLevel;
    private bool lost;
    private bool noBeacons;
    private int count;
	private float score;
    private bool inHSMenu;
    // Use this for initialization
    void Start () {
		print("UI manager start called");
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
        surviveLossMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().surviveLossMenu;
        highscoreMenu = GameObject.Find("InGameUI").GetComponent<UIManager>().highscoreMenu;
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
		levelname = ls.levelname;

        return; }

	// Update is called once per frame
	void Update () {
		if (!lost && !wonLevel) { 
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
			UpdateUI();
		}
        if (lost & !highscoreMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            //Player Lost Show Lost Menu
            if (levelType == 2) {
                surviveLossMenu.SetActive(true);
                score = Time.timeSinceLevelLoad;
                GameObject scoreresult = GameObject.Find("Score2");
                scoreresult.GetComponent<Text>().text = "You survived for: " + score.ToString();
            }
            else
            {
                lossMenu.SetActive(true);
            }
            
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
                if (count >= numToWin) { 
					winMenu.SetActive(true);
				}
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
				wonGame();
            }


        }
        else if (levelType == 2)//2 - Avoid Enemies for X time
        {

            countdownText.text = string.Format("Time: {0}", Time.timeSinceLevelLoad);
            //Case of Win
            if (Time.timeSinceLevelLoad <= 5)
            { //Display Objective
                winText.text = string.Format("Survive as long as possible!");

            }
            else
            {
                winText.text = "";
            }

        }
        else if (levelType == 3)//3- Traverse Maze collecting X pickups and Return to the Beginning
        {
            countdownText.text = string.Format("Time so far: {0}", Time.timeSinceLevelLoad);
            SetCountText();
            if (Time.timeSinceLevelLoad <= 5)
            { //Display Objective
                winText.text = string.Format("Find the gem in the maze \n and then find your way back!");
			} else if (Time.timeSinceLevelLoad <= 10) {
				winText.text = string.Format("Press B to drop beacons \n Use them as a breadcrumb trail!");
			} else if (count >= numToWin) {
				score = Time.timeSinceLevelLoad;
				wonGame();
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
	void wonGame (){
		wonLevel = true;
		winMenu.SetActive(true);
		Time.timeScale = 0.0f;
 		GameObject scoreresult = GameObject.Find("Score");
		scoreresult.GetComponent<Text>().text = "Your Score: " + score.ToString();
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



	public void AddScore(){
		float newScore = score;
        string newName;
        if (levelType == 2)
        {newName = inputName2.GetComponent<Text>().text;}
        else
        { newName = inputName.GetComponent<Text>().text; }
		float oldScore;
		string oldName;
		for(int i=0;i<5;i++){
			oldName = PlayerPrefs.GetString(i+levelname+"HScoreName");
            oldScore= PlayerPrefs.GetFloat(i + levelname + "HScore");
            if (oldName == "" && oldScore ==0){ //If the score hasn't been set yet
				PlayerPrefs.SetFloat(i+ levelname +"HScore",newScore);
				PlayerPrefs.SetString(i+ levelname +"HScoreName",newName);
				newScore = 0;
				newName = "";
				continue;
			}
			if(PlayerPrefs.HasKey(i+ levelname +"HScore")){
				if(levelType == 1 || levelType == 2){ //Higher is better
					if(PlayerPrefs.GetFloat(i+ levelname +"HScore")<newScore){ 
						// new score is higher than the stored score
						oldScore = PlayerPrefs.GetFloat(i+ levelname +"HScore");
						oldName = PlayerPrefs.GetString(i+levelname+"HScoreName");
						PlayerPrefs.SetFloat(i+ levelname +"HScore",newScore);
						PlayerPrefs.SetString(i+ levelname +"HScoreName",newName);
						newScore = oldScore;
						newName = oldName;
					}
				} else {
					if(PlayerPrefs.GetFloat(i+ levelname +"HScore")>newScore){ //Lower is better
						// new score is lower than the stored score
						oldScore = PlayerPrefs.GetFloat(i+ levelname +"HScore");
						oldName = PlayerPrefs.GetString(i+levelname+"HScoreName");
						PlayerPrefs.SetFloat(i+ levelname +"HScore",newScore);
						PlayerPrefs.SetString(i+ levelname +"HScoreName",newName);
						newScore = oldScore;
						newName = oldName;
					}
				}
			}else{ //The first time initialize the scores to 0 and ""
				PlayerPrefs.SetFloat(i+ levelname +"HScore",newScore);
				PlayerPrefs.SetString(i+ levelname +"HScoreName",newName);
				newScore = 0;
				newName = "";
			}
		}
	}

	public void LoadHScore(){
        inHSMenu = true;
		highscoreMenu.SetActive(true);
		winMenu.SetActive(false);
		pauseMenu.SetActive(false);
		lossMenu.SetActive(false);
        surviveLossMenu.SetActive(false);
		Text[] highscoreTexts = highscoreMenu.GetComponentsInChildren<Text>();
		for(int i =0; i < 5; i++){
			highscoreTexts[2*i + 3].text = PlayerPrefs.GetString(i+levelname+"HScoreName");
			highscoreTexts[2*i + 4].text = PlayerPrefs.GetFloat(i+levelname+"HScore").ToString();
		}
		highscoreTexts[13].text = inputName.GetComponent<Text>().text;
		highscoreTexts[14].text = score.ToString();
	}
}
