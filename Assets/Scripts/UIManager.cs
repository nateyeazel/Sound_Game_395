using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
    public Canvas UI;
    public Text winText;
    public Text countText;
	public Text scoreText;
    public Text countdownText;
    private GameObject player;
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
        gm.countChanged += SetCountText;
		gm.scoreChanged += SetScoreText;
		gm.youLost += SetGameOver;
        winText.text = "";
        countdownText.text = string.Format("Time until blindness: {0}", Mathf.Max(30 - Time.timeSinceLevelLoad, 0));
        countText.text = "Collected: 0";
        scoreText.text = "Score: 0"; 

    }
	
	// Update is called once per frame
	void Update () {
        //limited to 1 second
		if(Time.timeSinceLevelLoad <= 30)
        {
            countdownText.text = string.Format("Time until blindness: {0}", 30 - Time.timeSinceLevelLoad);
        } else {
            countdownText.text = string.Format("");
        }
	}

	void SetGameOver (string lossType){
        winText.text = "Loser, hit R to restart";
	}

	void SetCountText (int newCount)
	{
		count = newCount;
		countText.text = "Collected: " + count.ToString ();
		if (count >= 15)
		{
			winText.text = "You Win!";
		}
	}
	void SetScoreText(int newScore)
	{
		score = newScore;
		scoreText.text = "Score: " + score.ToString();
        count += 1;
        SetCountText(count);

	}
}
