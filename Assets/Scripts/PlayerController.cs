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
	//public event Action collectedMovingTarget = delegate{};

    //Game Manager Events
    public delegate void collectedMovingTargetEvent(float spawnTime);
    public event collectedMovingTargetEvent collectedMovingTarget;
    public delegate void ItemCollectedEvent(float spawnTime);
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
            float spawnTime = other.gameObject.GetComponent<PickUp>().spawnTime;
            itemCollected.Invoke(spawnTime);

		}

        if (other.gameObject.CompareTag("Enemy"))
        {
			hitEnemy.Invoke();
        }   
		if (other.gameObject.CompareTag ("Moving Target"))
		{
            source.Play();
            float spawnTime = other.gameObject.GetComponent<PickUp>().spawnTime;
            collectedMovingTarget.Invoke(spawnTime);

            //float new_x = Random.Range(-10f, 10f);
            //float new_z = Random.Range(-10f, 10f);
            //other.gameObject.transform.position = new Vector3(new_x, 0.5f, new_z);
            //points += Mathf.Max((int)(100 - Mathf.Floor(Time.time - spawnTime) * 5), 0);            
            ////spawnTime = Time.time;
            //count += 1;

            source.Play();
		}
		if (other.gameObject.CompareTag ("Ground")){
			touchingGround = true;
		}
    }

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("Ground")){
			touchingGround = false;
		}
	}



}
