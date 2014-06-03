using UnityEngine;
using System.Collections;

public class VisibleByCamera : MonoBehaviour {

	public Camera cameraToCheck;

	public bool seen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		seen = false;
	}

	void OnWillRenderObject(){
		if(Camera.current == cameraToCheck){
			Debug.Log ("true");
			//Seen by Camera
			seen = true;
		}
	}
}
