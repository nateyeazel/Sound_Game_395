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
    private bool lost;
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
        if (!lost) { UpdateUI();}
        
	}

    void UpdateUI()
    {
        if(levelType == 1)//1 - Collect X ammount going blind after Y time                                            
        {

            if (Time.timeSinceLevelLoad <= timetoBlindness)
            {
                countdownText.text = string.Format("Time until blindness: {0}", timetoBlindness - Time.timeSinceLevelLoad);
                if (Time.timeSinceLevelLoad <= 5)
                { //Display Objective
                    winText.text = string.Format("Collect {0} to Win", numToWin);

                }
                
                else if (timetoBlindness - Time.timeSinceLevelLoad < 10 && timetoBlindness - Time.timeSinceLevelLoad >0)
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
            if (Time.timeSinceLevelLoad <= 5)
            { //Display Objective
                winText.text = string.Format("Avoid Enemies for {0} Seconds to Win", timeLimit);

            }

            else if (timeLimit - Time.timeSinceLevelLoad < 5 && timeLimit - Time.timeSinceLevelLoad > 0)
            {
                //if soonto lose
                winText.text = string.Format("Only {0} Seconds Left!", Mathf.RoundToInt( timeLimit- Time.timeSinceLevelLoad));
            }
            else if (timeLimit - Time.timeSinceLevelLoad < 0) {
                SetGameOver("Time!");
            }
            else
            {
                winText.text = "";
            }



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
        lost = true;
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
