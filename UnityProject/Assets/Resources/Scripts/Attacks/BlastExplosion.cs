using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BlastExplosion : MonoBehaviour {

	public string poolName;

	public float lifeTimer = 0.0f;

	public void reset(){
		lifeTimer = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		lifeTimer+= Time.deltaTime;
		if(lifeTimer >= particleSystem.startLifetime){
			GameObjectPool.Instance.Despawn(poolName, gameObject);
		}
	}
}
