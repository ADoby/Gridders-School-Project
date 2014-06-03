using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Blast : MonoBehaviour {

	private float currentSpeed;
	public float maxSpeed;
	public float speedChange;
    public string blastColor="";

	public LayerMask targetMask;

	private Vector3 direction;

	public string poolName;

	public int sparkleCount;

	private float lifeTimer = 0.0f;

	public bool active;
	public float stayTimer = 0.0f;

	public ParticleSystem particles;

	public float damage = 0.0f;

	public string PartikelPoolName = "";

	public void reset(){
		currentSpeed = 0.0f;
		lifeTimer = 0.0f;
		stayTimer = 0.0f;
		damage = 0.0f;
		active = true;
	    blastColor = "";
	}

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        rigidbody.AddForce(direction * maxSpeed, ForceMode.VelocityChange);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
	
	// Update is called once per frame
	void Update () {
		if(active){
			//Update Life Timer
			lifeTimer += Time.deltaTime;
			if(particles && lifeTimer >= particles.duration){
				rigidbody.velocity = Vector3.zero;
				active = false;
				return;
			}
			
			//Update Speed
			//currentSpeed = Mathf.Clamp(currentSpeed + speedChange * Time.deltaTime, 0, maxSpeed);
			
			//rigidbody.velocity = direction.normalized * currentSpeed;

		}else{
			if(particles){
				stayTimer += Time.deltaTime;
				if(stayTimer >= particles.startLifetime){
					GameObjectPool.Instance.Despawn(poolName, gameObject);
				}
			}else{
				GameObjectPool.Instance.Despawn(poolName, gameObject);
			}
		}
	}
	

	void OnTriggerEnter(Collider info){
        if (active && IsInLayerMask(info.gameObject, targetMask))
        {
			//Deactivate the blast
			rigidbody.velocity = Vector3.zero;
			active = false;
			//Do Damage
			TargetAble targetScript = info.GetComponent<TargetAble>();
			if(targetScript)
				targetScript.doDamage(damage, blastColor);

			//Do Explosion
			if(PartikelPoolName != ""){
				GameObject go = GameObjectPool.Instance.Spawn(PartikelPoolName,  transform.position, Quaternion.identity);
				go.particleSystem.Play();
				go.GetComponent<BlastExplosion>().reset ();
				go.GetComponent<BlastExplosion>().poolName = PartikelPoolName;
			}

		}
	}

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        // Convert the object's layer to a bitfield for comparison
        int objLayerMask = (1 << obj.layer);
        if ((layerMask.value & objLayerMask) > 0) // Extra round brackets required!
            return true;
        else
            return false;
    }
}
