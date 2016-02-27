using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUp : MonoBehaviour {
    public float spawnTime;
    float timeToCollect;
    public string type;
    public int maxLocations;
    public Vector2 Xrange;
    public Vector2 Yrange;
    public List<Vector3> list = new List<Vector3>();
    private int index;
    // Use this for initialization
    void Start () {
        index = 0;
        spawnTime = Time.time;
        //initiate type (random moving, static, list moving)?
	}
	
	// Update is called once per frame
    public void MoveToNext(bool random)
    {
        if (random)
        {
            float new_x = UnityEngine.Random.Range(-10f, 10f);
            float new_z = UnityEngine.Random.Range(-10f, 10f);
            this.transform.position = new Vector3(new_x, 0.5f, new_z);

        }
        else
        {
            if (index < list.Capacity)
            {
                this.gameObject.transform.position = list[index];
                index += 1;
            }
            if (index == list.Capacity)
            {
//                this.gameObject.SetActive(false);
            }
        }
    }
}
