using UnityEngine;
using System.Collections;

public class LightMovie : MonoBehaviour {
	
	private Vector3 startPos;
	
	public Vector3 movement;
	
	public float speed;
	
	public Vector2 minMaxTime;
	
	public bool upOnly;
	
	private float timer = 0;
	private Vector3 newPos;
	// Use this for initialization
	void Start () {
		startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
		timer -= Time.deltaTime;
		if(timer<=0){
			newPos = startPos;
			if(!upOnly){
				newPos.x += Random.Range (-movement.x, movement.x);
				newPos.y += Random.Range (-movement.y, movement.y);
				newPos.z += Random.Range (-movement.z, movement.z);
			}else
				newPos.y += Random.Range (0, movement.y);
			timer = Random.Range (minMaxTime.x,minMaxTime.y);
		}
		
		transform.localPosition = Vector3.Slerp (transform.localPosition, newPos, Time.deltaTime * speed);
	}
}
