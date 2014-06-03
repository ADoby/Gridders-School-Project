using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class PlayerAttackManager : TargetAble {

	private float coolDown = 0.0f;

	public Transform camera;

	public Transform shootPosition;

	public string attackPoolName = "";
    //public string playerColor = ""; -> taken from TargetAble

	public AudioSource needleGun;
	public AudioSource heartBeat;

	// Use this for initialization
	void Awake () {
		coolDown = 0.0f;
	}

	// Update is called once per frame
	void Update () {

        //Things To make something cool
		if (false)
		{
			//heartBeat.Play();
			heartBeat.volume = Mathf.Lerp (heartBeat.volume, 0.5f, Time.deltaTime * 2.0f);
		}else{
			heartBeat.volume = Mathf.Lerp (heartBeat.volume, 0.0f, Time.deltaTime * 2.0f);
		}

		coolDown += Time.deltaTime;

		if (coolDown >= 0.25f)
		{
			if (Input.GetButton ("Fire1"))
			{
				coolDown = 0.0f;
				Vector3 targetPos = shootPosition.position - shootPosition.forward;

				//if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
				//	if(hit.collider.gameObject.tag != "Player"){
				//		targetPos = hit.point;
				//	}
				//}

				if (attackPoolName != "") {
					GameObject needle = GameObjectPool.Instance.Spawn (attackPoolName, shootPosition.position, Quaternion.identity);

					if(needle){
						needle.GetComponent<Blast> ().reset ();
						needle.GetComponent<Blast> ().poolName = attackPoolName;
						
						RaycastHit hit;
						Vector3 targetPosition = (camera.position + camera.forward * 50.0f);
						
						if (Physics.Raycast (camera.position, camera.forward, out hit, 50.0f)) {
							targetPosition = hit.point;
						}

                        needle.GetComponent<Blast>().SetDirection(targetPosition - shootPosition.position);
						needle.GetComponent<Blast> ().damage = 30.0f;
					}


					needleGun.Play();
				}
			}
		}
	}

	public override void doDamage(float damage, string damageColor)
	{
		base.doDamage(damage, damageColor);

        if (String.Equals(damageColor, targetAbleColor))
        {
			//uiManager.PlayerHit(damage);
        }
   	}
}
