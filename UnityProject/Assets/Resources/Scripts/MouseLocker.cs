using UnityEngine;
using System.Collections;

public class MouseLocker : MonoBehaviour {

    public bool lockCursorOnStart = true;

	// Use this for initialization
	void Start () {
        Screen.lockCursor = lockCursorOnStart;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("ESC"))
            Screen.lockCursor = false;
        else if (Input.GetMouseButtonDown(0))
            Screen.lockCursor = true;
	}
}
