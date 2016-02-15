using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject mainmenu;
    public GameObject levelsmenu;
    public GameObject optionsmenu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void loadlevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void options()
    {
        mainmenu.SetActive(false);
        optionsmenu.SetActive(true);
    }

    public void levelselect()
    {
        mainmenu.SetActive(false);
        levelsmenu.SetActive(true);
    }

    public void back()
    {
        mainmenu.SetActive(true);
        levelsmenu.SetActive(false);
        optionsmenu.SetActive(false);
    }
    public void quit() 

    {
        Application.Quit(); 

    }

}
