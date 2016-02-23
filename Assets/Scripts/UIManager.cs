using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
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
		gm.countChanged += SetCountText;
		gm.scoreChanged += SetScoreText;
		gm.youLost += SetGameOver;

		count = 0;
		score = 0;
		winText.text = "";
		countdownText.text = string.Format("Time until blindness: {0}", Mathf.Max(30 - Time.timeSinceLevelLoad, 0));

		SetCountText (count);
		SetScoreText(score);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad >= 30){
			countdownText.text = string.Format("");
		} else {
			countdownText.text = string.Format("Time until blindness: {0}", 30 - Time.timeSinceLevelLoad);
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
		scoreText.text = "Score: " + ((int)points).ToString();

	}
}
