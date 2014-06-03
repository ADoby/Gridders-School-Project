using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class MinionDeathExplosion : MonoBehaviour {

	private float lifeTimer = 0.0f;

	// Use this for initialization
	void Start () {
		lifeTimer = particleSystem.startLifetime + particleSystem.duration;
	}

	public void reset(){
		lifeTimer = particleSystem.startLifetime + particleSystem.duration;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTimer -= Time.deltaTime;
		if(lifeTimer <= 0){
			GameObjectPool.Instance.Despawn("MinionDeath", gameObject);
		}
	}
}
