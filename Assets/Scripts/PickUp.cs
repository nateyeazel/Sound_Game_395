using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUp : MonoBehaviour {
    public float spawnTime;
    public int type=1;
    //1 - Static -Default
    //2 - Random Moving
    //3 - List Moving
    //4 - Random List?
    //public int maxLocations;
    public Vector2 Xrange;//
    public Vector2 Yrange;//
    public List<Vector3> list = new List<Vector3>();
    private int index = 0;
    // Use this for initialization
    void Start () {
        spawnTime = Time.time;
	}
	
	// Update is called once per frame
    public void PickedUp()
    {
        if (type== 1) {
            //Static Pickup just disable
            this.gameObject.SetActive(false);
        }
        else if (type == 2)
        {
            //List Behavior
            if (index < list.Capacity - 1)
            {
                this.gameObject.transform.position = list[index];
                index += 1;
            }
            else if (index >= list.Capacity -1)//once end of list is reached disable pick up
            {
                this.gameObject.SetActive(false);
            }

        }
        else  if (type == 3)
        {
            //Move to Random (Maybe Set Limit?)
            float new_x = UnityEngine.Random.Range(Xrange[0], Xrange[1]);
            float new_z = UnityEngine.Random.Range(Yrange[0], Yrange[1]);
            this.transform.position = new Vector3(new_x, 0.5f, new_z);

        }
        else
        {
            //Default to Normal Pickup Behavior
            this.gameObject.SetActive(false);
        }
    }
}
