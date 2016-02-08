using UnityEngine;
using System.Collections;

public class CircleMovement : MonoBehaviour {
    float angle = 0;
    float speed;
    float x, z;
    public float totaltime, radius;
    //public Vector2 center;

	// Use this for initialization
	void Start () {
        this.speed = (2 * Mathf.PI) / totaltime;
    }
	
	// Update is called once per frame
	void Update () {
        angle += this.speed * Time.deltaTime;
        x = Mathf.Cos(angle) * radius;
        z = Mathf.Sin(angle) * radius;
        transform.position = (new Vector3(x, transform.position.y, z));
    }
}
