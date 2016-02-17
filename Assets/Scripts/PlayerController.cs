using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text winText;
	public Text countText;
    public Text scoreText;
    public Text countdownText;
	private Rigidbody rb;
    private GameObject Maincamera;
    private int count;
    private int points;
	private bool gameOver;
	private bool visibleObjects;
	public Vector3 JumpVelocity;
    public AudioClip itemcollected;
    private AudioSource source;
    private float spawnTime;
    public bool flicker;

	void Start ()
	{
        flicker = false;
        source = gameObject.GetComponent<AudioSource>();
        source.clip = itemcollected;
        rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
        spawnTime = Time.time;
        points = 0;
        SetPointsText();
        gameOver = false;
		visibleObjects = true;
        Maincamera = GameObject.Find("Main Camera");
        
        
	}

	void Update () {
		
		countdownText.text = string.Format("Time until blindness: {0}", Mathf.Max(30 - Time.realtimeSinceStartup, 0));

		if(Time.realtimeSinceStartup >= 30){
            countdownText.text = string.Format("");
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
		} 

		if(Input.GetButtonDown("Jump")) {
			this.GetComponent<Rigidbody>().AddForce(JumpVelocity, ForceMode.VelocityChange);
		}

		if(rb.position.y <= -10) {
			gameOver = true;
            Time.timeScale = 0;
            winText.text = "Loser, hit R to restart";
		}

		if(gameOver && Input.GetKeyDown("r")){
			SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
            Time.timeScale = 1;
        }
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        movement = Maincamera.transform.rotation * movement;


        rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            source.Play();
            other.gameObject.SetActive (false);
			count = count + 1;
            source.Play();
            SetCountText ();
		}

        if (other.gameObject.CompareTag("Enemy"))
        {
            winText.text = "Loser, hit R to restart";
            Time.timeScale = 0;
            gameOver = true;
        }   
		if (other.gameObject.CompareTag ("Moving Target"))
		{
			float new_x = Random.Range(-10f, 10f);
			float new_z = Random.Range(-10f, 10f);
			other.gameObject.transform.position = new Vector3(new_x, 0.5f, new_z);
            points += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);            
            count += 1;
            spawnTime = Time.time;
            source.Play();
			SetCountText ();
            SetPointsText();
		}
    }

	void SetCountText ()
	{
		countText.text = "Collected: " + count.ToString ();
		if (count >= 15)
		{
			winText.text = "You Win!";
		}
	}
    void SetPointsText()
    {
        scoreText.text = "Score: " + ((int)points).ToString();
        
    }
}
