using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SpellController : MonoBehaviour {
	
	public Spell spell;
	
	public float lifeTime;
	private float lifeTimer;
	
	private bool spawned;
	private bool released;
	
	private PlayerController owner;
	
	private bool noDMG;
	
	public bool destroyOnImpact;
	
	public string poolName = "";
	
	public bool soundOnDestroy;
	public GameObject soundDestroy;

	public void init(){
		spawned = false;
		released = false;
		owner = null;
		noDMG = false;

		spell.init(transform, transform.localScale);
		
		//If Spell is a Sparkle it moves itself directly after spawning
		if(spell.spellType == Constants.SpellType.Sparkle){
			release();
		}
		
		lifeTimer = spell.getSpawnTime();
		
		//rigidbody.freezeRotation = true;
	}
	
	void FixedUpdate(){
		if(spawned){
			//move
			if(released){			
				
				if(spell.wantsToBeRotated()){
					//Get Away from midlepoint
					if(transform.localPosition.magnitude <= 1f)
						rigidbody.AddForce (transform.localPosition.normalized * spell.speedOverTime);
				}else{
					rigidbody.AddForce (rigidbody.velocity.normalized * spell.speedOverTime);
				}
				
				
			}
		}
	}
	
	public void setNoDmg(bool b){
		noDMG = b;	
	}
	
	// Update is called once per frame
	void Update () {
		if(spawned){
			if(spell.releaseWithMouse){
				if(Input.GetMouseButtonDown (0)){
                    if (owner) { }
						//owner.ReleaseSpell(transform);
				}
			}
			
			lifeTimer += Time.deltaTime;

			if(lifeTimer >= lifeTime){
				DestroySpell();
				return;
			}
			
		}else if(!spawned){
			spawned = spell.updateSpawner();
			if(spawned){
				//Happens only one time
				finishedSpawning();
			}
			if(spell.spellType == Constants.SpellType.Bomb){
				if(spell.releaseWithMouse){
					if(Input.GetMouseButtonDown (0)){
                        if (owner) { }
							//owner.ReleaseSpell(transform);
					}
				}
			}
		}
	}

	bool alive = true;

	private void DestroySpell(){
		if(spell.spawnSparklesOnDetonate){
			spawnSparkles();
		}
		//if(!released && owner) owner.ReleaseSpell(transform);
		if(soundOnDestroy){
			Instantiate(soundDestroy, transform.position, Quaternion.identity);	
		}

		GameObjectPool.Instance.Despawn(poolName, gameObject);

		//Destroy (gameObject);
	}

	private void DestroySpell(Collider info){
		if(!noDMG && spell.properTarget(info.collider.gameObject.layer)){
			if(info.gameObject.GetComponent<TargetAble>()){
				info.gameObject.GetComponent<TargetAble>().doDamage(spell.damage, "");
			}
			DestroySpell();
		}else if(destroyOnImpact){
			DestroySpell();
		}else{
			if(spell.spawnSparklesOnImpact){
				spawnSparkles();
			}
		}
	}
	
	void OnTriggerEnter(Collider info){
		DestroySpell (info);
	}
	
	public void finishedSpawning(){
		//if(owner) owner.SpellFinishedSpawning(transform, spell.holdAfterSpawn, spell.wantsToBeInIceStorm, spell.wantsToBeInFireStorm);
		
		//Spell specific things
		if(spell.spellType == Constants.SpellType.Bomb){
			spawnSparkles();
			renderer.enabled = false;
			light.enabled = false;
			//if(owner) owner.ReleaseSpell(transform);
		}else if(spell.spellType == Constants.SpellType.Storm){
			spawnSparkles();
			renderer.enabled = false;
			light.enabled = false;
			//if(owner) owner.ReleaseSpell(transform);
		}
	}



	private void spawnSparkles(){
		//Spawn some Sparkles !!
		GameObject go;
		for(int i = 0;i <spell.sparkleCount;i++){

			go = GameObjectPool.Instance.Spawn("BlastSparkle",  transform.position, Quaternion.identity);

			go.GetComponent<SpellController>().init();
			go.GetComponent<SpellController>().setNoDmg(spell.SparklesAreEffect);
			go.GetComponent<SpellController>().poolName = "BlastSparkle";
			go.GetComponent<SpellController>().ReleaseWithForce(Random.onUnitSphere);
		}
	}
	
	public void updateParent(){
		//if(owner) owner.SpellFinishedSpawning(transform, spell.holdAfterSpawn, spell.wantsToBeInIceStorm, spell.wantsToBeInFireStorm);
	}
	
	public void ReleaseWithForce(Vector3 direction){
		spell.instantSpawn();
		spawned = true;
		if(spell.spawnSparklesOnRelease){
			spawnSparkles();	
		}
		released = true;

		rigidbody.AddForce(direction.normalized * spell.releaseSpeed);
	}

	public void release(){
		
		if(spell.spellType == Constants.SpellType.Ball){
			spell.instantSpawn();
			spawned = true;
		}else if(spell.spellType == Constants.SpellType.Sparkle){
			spell.instantSpawn();
			spawned = true;
		}
		
		if(spell.spawnSparklesOnRelease){
			spawnSparkles();	
		}
		
		released = true;
		rigidbody.freezeRotation = false;
		rigidbody.AddForce(transform.forward * spell.releaseSpeed);
		
		//Spell Finished here, no control from player at this point anymore
	}
	
	void OnCollisionExit(){
		rigidbody.AddForce (rigidbody.velocity.normalized * spell.speedBoostOnCollide);
	}
	
	public void SetOwner(PlayerController controler){
		owner = controler;
		
		//Spell specific things
		if(spell.spellType == Constants.SpellType.Bomb){
			//owner.SpellFinishedSpawning(transform, spell.holdAfterSpawn, false, false);
		}
	}
}
