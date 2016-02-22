using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform target;
    public float speedmultiplier;
    public float radius; 
    private Transform mytransform;


    void Awake()
    {
        mytransform = transform;
    }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(target.position,mytransform.position) < radius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speedmultiplier);

//            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime*speedmultiplier );
        }

    }
}
