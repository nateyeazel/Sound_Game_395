using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;
    private GameObject Maincamera;
	private bool touchingGround;
	public Vector3 JumpVelocity;
    public AudioClip itemcollectedsound;
    private AudioSource source;
    private float spawnTime;
    
    //general events actions
	//public event Action itemCollected = delegate{};
	public event Action hitEnemy = delegate{};
	public event Action fellOff = delegate{};
	public event Action lightsOff = delegate{};
	//public event Action collectedMovingTarget = delegate{};

    //Game Manager Events
    public delegate void collectedMovingTargetEvent(GameObject movingTarget);
    public event collectedMovingTargetEvent collectedMovingTarget;
    public delegate void ItemCollectedEvent(GameObject item);
    public event ItemCollectedEvent itemCollected;


    void Start ()
	{
        source = gameObject.GetComponent<AudioSource>();
        source.clip = itemcollectedsound;
        rb = GetComponent<Rigidbody>();
		touchingGround = true;
        Maincamera = GameObject.Find("Main Camera");     //to be used for movement
        
	}

	void Update () {
		if(rb.position.y <= -10) {
			fellOff.Invoke();
		}
	}

	void OnCollisionStay () {
		//Jump effects.
		if (Input.GetButtonDown ("Jump")) {
			this.GetComponent<Rigidbody>().AddForce(JumpVelocity, ForceMode.VelocityChange);
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if(!touchingGround){ //Not currently working... 
			movement *= 0.1f;
		}
        movement = Maincamera.transform.rotation * movement;


        rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            source.Play();
            other.gameObject.SetActive (false);
            itemCollected.Invoke(other.gameObject);

		}

        if (other.gameObject.CompareTag("Enemy"))
        {
			hitEnemy.Invoke();
        }   
		if (other.gameObject.CompareTag ("Moving Target"))
		{
            source.Play();
            collectedMovingTarget.Invoke(other.gameObject);
		}
		if (other.gameObject.CompareTag ("Ground")){
			touchingGround = true;
		}
		if (other.gameObject.CompareTag ("Lights Off")){
			lightsOff.Invoke();
		}
    }

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("Ground")){
			touchingGround = false;
		}
	}



}
