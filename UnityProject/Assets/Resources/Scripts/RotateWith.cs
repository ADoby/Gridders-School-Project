using UnityEngine;
using System.Collections;

public class RotateWith : MonoBehaviour {

	
	public float speedX;
	
	public float maxChangeX;
	
	private Vector3 startRot;
	
	public float speedY;
	public float maxChangeY;
	
	public float minX;
	public float maxX;
	
	// Use this for initializations
	void Start () {
		startRot = transform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = Quaternion.Lerp (transform.localRotation,
													Quaternion.Euler(startRot + Vector3.up * Input.GetAxis ("Mouse X") * maxChangeX ),
													Time.deltaTime * speedX);
		
		
		
		Vector3 change = (Vector3.right * Input.GetAxis ("Mouse Y") * maxChangeY) * Time.deltaTime * speedY;
		
		//Hoch Runter
		if(change.x < 0){
			if((transform.localRotation.eulerAngles - change).x < maxX){
				transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - change);
			}else{
				transform.localRotation = Quaternion.Euler(	new Vector3(maxX,
																		transform.localRotation.eulerAngles.y,
																		transform.localRotation.eulerAngles.z));
			}
		}else if(change.x > 0){
			if((transform.localRotation.eulerAngles - change).x > minX){
				transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - change);
			}else{
				transform.localRotation = Quaternion.Euler(	new Vector3(minX,
																		transform.localRotation.eulerAngles.y,
																		transform.localRotation.eulerAngles.z));
			}
		}
		
		
	}
}
