using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	
	private PlayerController controller;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		GUI.TextField (new Rect(0.1f,0.1f,controller.health*2,20), "");	
		GUI.TextField (new Rect(0.1f,20,50,20), controller.health.ToString ());	
	}
}
