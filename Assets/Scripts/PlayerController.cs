﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text winText;

	private Rigidbody rb;
	private int count;
	private bool gameOver;

	public Vector3 JumpVelocity;


	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
//		SetCountText ();
//		winText.text = "";
		gameOver = false;
	}

	void Update () {

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

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
			count = count + 1;
//			SetCountText ();
		}

        if (other.gameObject.CompareTag("Enemy"))
        {
            winText.text = "Loser, hit R to restart";
            Time.timeScale = 0;
            gameOver = true;
            if (gameOver && Input.GetKeyDown("r"))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }
    }

//	void SetCountText ()
//	{
//		countText.text = "Count: " + count.ToString ();
//		if (count >= 7)
//		{
//			winText.text = "You Win!";
//		}
//	}
}
