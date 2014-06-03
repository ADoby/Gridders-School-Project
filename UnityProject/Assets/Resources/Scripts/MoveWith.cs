using UnityEngine;
using System.Collections;

public class MoveWith : MonoBehaviour {
	
	public Transform target;
	
	public bool x;
	public bool y;
	public bool z;
	
	public bool keepDifference;
	
	private Vector3 diff;
	
	public bool smooth;
	public float speed;
	
	public bool justFollow;
	
	private bool msgSend;
	
	// Use this for initialization
	void Start () {
		diff = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(!target && !msgSend){
			Debug.LogWarning("You have to give me a Target: " + gameObject);	
			msgSend = true;
			return;
		}
		if(justFollow){
			transform.position = target.position;
			return;
		}
		Vector3 newPos = transform.position;
		newPos.x = x ? target.position.x : newPos.x;
		newPos.y = y ? target.position.y : newPos.y;
		newPos.z = z ? target.position.z : newPos.z;
		if(keepDifference)
			newPos += target.TransformDirection(diff);
		if(!smooth){
			transform.position = newPos;
		}else{
			transform.position = Vector3.Slerp (transform.position, newPos, Time.deltaTime * speed);
		}

	}
}
