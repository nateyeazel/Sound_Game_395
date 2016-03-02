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
    private GameObject player;
    //Level Settings
    private int levelType;
    private int timeLimit;
    private int numToWin;
    private int timetoBlindness;

	private bool wonLevel;
    private int count;
	private int score;
    
    // Use this for initialization
    void Start () {
		GameObject gameManagerObject = GameObject.Find("GameManager");
		GameManager gm = gameManagerObject.GetComponent<GameManager>();
        winText = GameObject.Find("InGameUI").GetComponent<UIManager>().winText;
        countdownText = GameObject.Find("InGameUI").GetComponent<UIManager>().countdownText;
        countText = GameObject.Find("InGameUI").GetComponent<UIManager>().countText;
        scoreText = GameObject.Find("InGameUI").GetComponent<UIManager>().scoreText;
		gm.scoreChanged += updateScoreCount;
        gm.UISetup += SetupUI;
		gm.youLost += SetGameOver;
		wonLevel = false;
        winText.text = "";
		countdownText.text = "";
        countText.text = "";
        scoreText.text = ""; 

    }
	
    void SetupUI(GameObject LevelSettings) {
        //Get Level Settings including type numToWin etc.
        LevelSettings ls = LevelSettings.GetComponent<LevelSettings>();
        levelType = ls.levelType;
        numToWin = ls.numWin;
        timeLimit = ls.timeLimit;
        timetoBlindness = ls.timeToBlindness;
        

        return; }

	// Update is called once per frame
	void Update () {
		if (wonLevel) {
			if(Input.GetKeyDown("m")){
				SceneManager.LoadScene("MainMenu");
				Time.timeScale = 1;
			}

			if(Input.GetKeyDown("r")){
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				Time.timeScale = 1;
			}
		}
        UpdateUI();
	}

    void UpdateUI()
    {
        if(levelType == 1)//1 - Collect X ammount going blind after Y time                                            
        {
            if (Time.timeSinceLevelLoad <= timetoBlindness)
            {
                countdownText.text = string.Format("Time until blindness: {0}", timetoBlindness - Time.timeSinceLevelLoad);
            }
            else {
                countdownText.text = string.Format("");
            }
            SetCountText();
            SetScoreText();
            //Case of Win
            if (count >= numToWin) { 
				wonLevel = true;
				winText.text = "You Win! \n Press M to return to the main menu or R to retry"; 
			}

        }
        else if(levelType == 2)//2 - Avoid Enemies for X time
        {

            countdownText.text = string.Format("Time left: {0}", timeLimit - Time.timeSinceLevelLoad);
            //Case of Win
            if (Time.timeSinceLevelLoad > timeLimit)
            {
                countdownText.text = string.Format("");
				winText.text = "You Win! \n Press M to return to the main menu or R to retry";
				wonLevel = true;
            }

        }
        else if (levelType == 3)//3- Traverse Maze collecting X pickups and Return to the Beginning
        {
            countdownText.text = string.Format("Time so far: {0}", Time.timeSinceLevelLoad);
            SetCountText();

            //Case of Win
            if (count >= numToWin)   {
				wonLevel = true;
				winText.text = "You Made it through the maze! \n Press M to return to the main menu or R to retry";
			}

        }

    }

	void SetGameOver (string lossType){
        winText.text = "Loser, hit R to restart";
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
}
