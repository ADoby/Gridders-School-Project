using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

    public VideoMenu videoMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("ESC"))
        {
            Screen.lockCursor = false;
            videoMenu.enabled = true;
        }
	}
}
