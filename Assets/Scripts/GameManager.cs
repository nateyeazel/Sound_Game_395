using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private bool gameOver;
    private bool paused = false;
    private float difficulty;
    private Canvas MainMenu;
    private Canvas UI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
            }



        }

       

    }
}
