using UnityEngine;
using System.Collections;


public class LevelSettings : MonoBehaviour {
    public int levelType = 1; //default to level type 1
                              //1 - Collect X ammount going blind after Y time
                              //2 - Avoid Enemies for X time
                              //3- Traverse Maze collecting X pickups and Return to the Beginning

    //Number of pickups needed to win;
    public int numWin = 10;// Default to 10 
    // How Long before objects stop appearing visually
    public int timeToBlindness=30;//Default to 30 seconds
    //Number of Beacons allowed
    public int numBeacons;
    //Time limit for enemy avoidance / timed mazes?
    public int timeLimit = 30;//Default to 30 seconds

    // Use this for initialization
    void Start () {
	
	}
}
