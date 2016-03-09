using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {
    Vector3 target;
    float speedmultiplier;
    float createdtime;
    bool spawn = false;
	// Use this for initialization
	void Start () {
        //Random Speed
        createdtime = Time.time;
        speedmultiplier = UnityEngine.Random.Range(2f, 5f);
        float new_x = UnityEngine.Random.Range(-10f, 10f);
        float new_z = UnityEngine.Random.Range(-10f, 10f);
        target = new Vector3(new_x, 0.5f, new_z);
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speedmultiplier);

        if (transform.position == target)
        {
            speedmultiplier = UnityEngine.Random.Range(2f, 5f);
            float new_x = UnityEngine.Random.Range(-10f, 10f);
            float new_z = UnityEngine.Random.Range(-10f, 10f);
            target = new Vector3(new_x, 0.5f, new_z);
        }
        //Spawn every 10 seconds
        if (Time.time > createdtime + 10)
        {
            createdtime = Time.time;
            Instantiate(this.gameObject);
            spawn = true;          
        }

    }
}
