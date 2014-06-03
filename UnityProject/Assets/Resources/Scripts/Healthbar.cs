using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {

	private Transform mainCam;

	public Transform bar;
	public GameObject AggroBar;

	private static float maxWidth = 8.0f;

	private float currentProcent = 1.0f;
	private float wantedProcent = 1.0f;

	public float speed = 2.0f;

	public void UpdateHealth(float procent){
		wantedProcent = procent;
	}

	public void setAggro(bool aggro){
		AggroBar.SetActive(aggro);
	}

	void Start(){
		mainCam = Camera.main.transform;
	}

	void Update(){
		currentProcent = Mathf.Lerp (currentProcent, wantedProcent, Time.deltaTime * speed);
		bar.localScale = new Vector3(currentProcent * maxWidth, 1,1);
		bar.localPosition = new Vector3((1.0f-currentProcent) * maxWidth/2.0f, 0,0);

		transform.LookAt(mainCam);
	}

}
