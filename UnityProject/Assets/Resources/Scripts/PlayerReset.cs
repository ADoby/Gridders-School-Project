using UnityEngine;
using System.Collections;

public class PlayerReset : MonoBehaviour {

    private Vector3 resetPosition;

	// Use this for initialization
	void Start () {
        resetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Reset"))
        {
            transform.position = resetPosition;
        }
	}
}
