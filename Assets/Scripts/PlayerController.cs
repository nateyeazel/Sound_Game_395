using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
    private GameObject Maincamera;
	public Vector3 JumpVelocity;
    public AudioClip itemcollectedsound;
    private AudioSource source;
    private float distToGround;
    public float maxSpeed;
    
    //general events actions
	//public event Action itemCollected = delegate{};
	public event Action hitEnemy = delegate{};
	public event Action fellOff = delegate{};
	//public event Action collectedMovingTarget = delegate{};

    //Game Manager Events
    public delegate void collectedMovingTargetEvent(GameObject movingTarget);
    public event collectedMovingTargetEvent collectedMovingTarget;
    public delegate void ItemCollectedEvent(GameObject item);
    public event ItemCollectedEvent itemCollected;
    public delegate void lightsOffEvent(GameObject item);
    public event lightsOffEvent lightsOff;

    void Start ()
	{
        distToGround = this.GetComponent<Collider>().bounds.extents.y;
        source = gameObject.GetComponent<AudioSource>();
        source.clip = itemcollectedsound;
        rb = GetComponent<Rigidbody>();
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

		if(!IsGrounded()){ //Not currently working... 
			movement *= 0.1f;
		}

        Quaternion camera = Maincamera.transform.rotation;
        movement = camera * movement;
        movement.y = 0;

        rb.AddForce(movement * speed, ForceMode.VelocityChange);
        //Max Speed Implemtation
        if (rb.velocity.magnitude > maxSpeed)
        {
            float brakeSpeed = rb.velocity.magnitude - maxSpeed;  // calculate the speed decrease
            Vector3 normalisedVelocity = rb.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value
            rb.AddForce(-brakeVelocity, ForceMode.Impulse);  // apply opposing brake force
        }
        
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            source.Play();
            itemCollected.Invoke(other.gameObject);
		}
        if (other.gameObject.CompareTag("Moving Target"))
        {
            source.Play();
            collectedMovingTarget.Invoke(other.gameObject);
        }
        if (other.gameObject.CompareTag("Lights Off"))
        {
            source.Play();
            lightsOff.Invoke(other.gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
			hitEnemy.Invoke();
        }   

    }


    Boolean IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
            }
 }
