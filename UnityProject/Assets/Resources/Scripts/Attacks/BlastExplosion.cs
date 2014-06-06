using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BlastExplosion : MonoBehaviour {

	public string poolName;

	public float lifeTimer = 0.0f;

	public void Reset(){
		lifeTimer = 0.0f;
	}

    public void SetPool(string poolName)
    {
        this.poolName = poolName;
    }

	// Update is called once per frame
	void Update () {
		lifeTimer+= Time.deltaTime;
		if(lifeTimer >= particleSystem.startLifetime){
			GameObjectPool.Instance.Despawn(poolName, gameObject);
		}
	}
}
