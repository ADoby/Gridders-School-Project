using UnityEngine;
using System.Collections;

public class WalkelLampe : MonoBehaviour {

	public Vector3 wakeln;
	
	private Vector3 startRot;
	
	public float speed;
	
	public float changePositionCooldown;
	
	private float positiontimer;
	
	private Vector3 newRot;
	// Use this for initialization
	void Start () {
		startRot = transform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		positiontimer-= Time.deltaTime;
		if(positiontimer<0){
			newRot.x = Random.Range(-wakeln.x, wakeln.x);
			newRot.y = Random.Range(-wakeln.y, wakeln.y);
			newRot.z = Random.Range(-wakeln.z, wakeln.z);
			positiontimer = changePositionCooldown;
		}
		
		transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler(startRot + newRot), Time.deltaTime * speed);
	}
}
