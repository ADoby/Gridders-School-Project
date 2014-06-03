using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	private Transform target;

	public bool smooth = false;
	public float speed = 1.0f;

	// Use this for initialization
	void Awake () {
		if(!target){
			this.enabled = false;
		}
	}

	public void changeTarget(Transform pTarget) {
		target = pTarget;
		if(!target)
			this.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
//        Debug.Log("LookAt.Update() target=" + target);

		Vector3 direction = target.position - transform.position;
		if(direction != Vector3.zero){
			if(smooth){
				Quaternion lookrot = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Lerp (transform.rotation, lookrot, Time.deltaTime * speed);
			}else{
				transform.LookAt(target);
			}
		}
	}
}
