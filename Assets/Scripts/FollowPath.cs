using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
    public Vector3 target;
    public float speedmultiplier;
    private Transform mytransform;
    public List<Vector3> list = new List<Vector3>();
    private int index = 0;

    void Awake()
    {
        mytransform = transform;

        //Translate to world coordinates
        for(int i = 0; i<list.Capacity; i++)
        {
            list[i] = list[i] + mytransform.position;
        }
    }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speedmultiplier);
        
        if(transform.position == target)
        {
            if (index < list.Capacity -1)
            {
                index += 1;

            }
            else {
                index = 0;
            }
        }
        target = list[index];

    }
}
