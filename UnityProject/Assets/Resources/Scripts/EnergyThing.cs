using UnityEngine;
using System.Collections;

public class EnergyThing : MonoBehaviour {
	
	public GoAwayBlock[] blocks;
	
	public int sparkleCount;
	public Transform sparkle;
	
	public bool SparklesAreEffect;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision){
		Debug.Log ("Hit by something");
		if(collision.collider.gameObject.tag == "Spell"){
			foreach(GoAwayBlock go in blocks){
				if(go)
					go.goAway = true;	
			}
			spawnSparkles();
			Destroy (gameObject);	
		}
	}
	
	private void spawnSparkles(){
		//Spawn some Sparkles !!
		GameObject go;
		for(int i = 0;i <sparkleCount;i++){
			int plussOrMinus = Random.Range(-1,1);
			if(plussOrMinus == 0) plussOrMinus = 1;
			
			go = (GameObject)Instantiate(sparkle.gameObject, transform.position + plussOrMinus*new Vector3(Random.Range(0f,1f),0, Random.Range(0f,1f)), Random.rotation);
			
			go.GetComponent<SpellController>().SetOwner(null);
			go.GetComponent<SpellController>().updateParent();
			go.GetComponent<SpellController>().setNoDmg(SparklesAreEffect);
			
		}
	}
}
