using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject mainmenu;
    public GameObject levelsmenu;
    public GameObject optionsmenu;
    public GameObject instructions;
	// Use this for initialization
	void Start () {
        Cursor.visible = true;

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

    public void instructionsmenu(){
        mainmenu.SetActive(false);
        instructions.SetActive(true);
    }
    public void back()
    {
        mainmenu.SetActive(true);
        levelsmenu.SetActive(false);
        optionsmenu.SetActive(false);
        instructions.SetActive(false);
    }
    public void quit() 

    {
        Application.Quit(); 

    }

}
