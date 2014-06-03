using UnityEngine;
using System.Collections;

public class Enemie_KI : TargetAble {

	public float FOVAngle = 110.0f;

	public float walkspeed = 2.0f;
	public float rotSpeed = 5.0f;

	public float aggroRange = 10.0f;
	public float shootRange = 10.0f;

	public float speedChange = 1.0f;

	public Transform EyePosition;

	float currentSpeed = 0.0f;

	TargetAble targetScript = null;

	public float currentAngleToRot = 0;

	public Vector3 lastKnownPosition;

	public float minDistance = 3.0f;

	private float maxHealth;

	void Start(){
		maxHealth = health;
		agent = GetComponent<NavMeshAgent>();
		StartCoroutine(checkTarget());
		agent.updateRotation = false;
		agent.stoppingDistance = 0.0f;
		lastKnownPosition = transform.position;
		positionTarget.position = lastKnownPosition;
		healthbar.UpdateHealth(health/maxHealth);
	}

	public override void doDamage(float damage, string color){
		base.doDamage(damage, color);
		healthbar.UpdateHealth(health/maxHealth);
		if(isDestroyed()){
			GameObjectPool.Instance.Despawn("Groder", gameObject);
			GameObject go = GameObjectPool.Instance.Spawn("MinionDeath", transform.position, Quaternion.identity);
			go.particleSystem.Play ();
			go.GetComponent<MinionDeathExplosion>().reset ();
		}
	}

	public Healthbar healthbar;

	public Transform shootPosition;
	public float shootTimer = 0.0f;
	public float shootCooldown = 1.0f;

    public string BlastPoolName;


	NavMeshAgent agent;

	public float wantedDistance = 5.0f;

	public Transform positionTarget;

	public float damage = 5.0f;

	private bool CheckSight(){
		RaycastHit hit;
		if(Physics.Raycast(EyePosition.position, (target.position - EyePosition.position), out hit)){
			if(hit.transform == target){
				return true;
			}
		}
		return false;
	}

	public void targeting(Transform attacker){
		if(attacker.tag == "PlayerMinion"){
			if(target.tag == "Player"){
				//If current target is player and attacker is a minion
				//Change current target to minion, because minions defend player
				targetScript = attacker.GetComponent<TargetAble>();
				if(targetScript)
					target = attacker;
			}
		}
	}

	private void AttackTarget(Vector3 targetPos){
		//Shoot
		if(shootPosition){
			if(shootTimer <= 0){
				shootTimer = shootCooldown;

                GameObject spell = GameObjectPool.Instance.Spawn(BlastPoolName, shootPosition.position, Quaternion.identity);
				
				spell.GetComponent<Blast>().reset ();
                spell.GetComponent<Blast>().poolName = BlastPoolName;
				spell.GetComponent<Blast>().SetDirection(targetPos-shootPosition.position);
				spell.GetComponent<Blast>().damage = damage;
				animation.Blend("Groder_Shoot");

				//Tell our enemy that we are attacking
				targetScript.targeting(transform);
			}
		}
	}

	IEnumerator checkTarget(){
		//Check if old Target is valid
		if(target){
			if(targetScript.isDestroyed()){
				target = null;
			}else if(Vector3.Distance(target.position, transform.position) > aggroRange){
				target = null;
			}
		}
		if(!target){
			Collider[] hitColliders = Physics.OverlapSphere(EyePosition.position, aggroRange);
			foreach(Collider coll in hitColliders){
				if(coll.tag == "Player" || coll.tag == "PlayerMinion"){
					//First Element with Tag Player/PlayerMinion gets Attacked/Targeted
					targetScript = coll.GetComponent<TargetAble>();
					if(targetScript){
						target = coll.transform;
						break;
					}
				}
			}
		}

		yield return new WaitForSeconds(0.2f);
		StartCoroutine(checkTarget ());
	}

	public float DistanceToPlayerDebug = 0.0f;
	public float hearDistance = 5.0f;

	// Update is called once per frame
	void Update () {
		shootTimer-= Time.deltaTime;

		if(target){

			Vector3 targetPos = target.position;

			//Get the difference angle between me look-direction and target-direction
			float angle = Vector3.Angle (transform.forward, targetPos-transform.position);

			float distanceToTarget = Vector3.Distance(targetPos, transform.position);
			DistanceToPlayerDebug = distanceToTarget;

			if(angle <= FOVAngle/2.0f){

				//Target is in front of me
				if(CheckSight()){
					healthbar.setAggro(true);

					//Now that we know we see the target, check distance
					//Because we don't wanna fight close :P

					Vector3 targetDirection = transform.forward * (wantedDistance-distanceToTarget);

					NavMeshHit hit;
					if(NavMesh.SamplePosition(transform.position - targetDirection, out hit, 5.0f, 1)){
						agent.destination = hit.position;
					};

					lastKnownPosition = targetPos;

					AttackTarget(targetPos);
					RotateTowards(targetPos);
				}else{
					if(distanceToTarget < hearDistance){
						lastKnownPosition = targetPos;
					}
					agent.destination = lastKnownPosition;
					RotateTowards(agent.destination);
				}
			}else{
				if(distanceToTarget < hearDistance){
					lastKnownPosition = targetPos;
				}
				agent.destination = lastKnownPosition;
				RotateTowards(agent.destination);
			}

			positionTarget.position = agent.destination;
		}else{
			healthbar.setAggro(false);
			RotateTowards(agent.destination);
		}
	}

	private void RotateTowards(Vector3 pos){
		if(pos != transform.position){
			Vector3 rot = Quaternion.LookRotation(pos - transform.position).eulerAngles;
			rot.x = 0;
			rot.z = 0;
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(rot), Time.deltaTime * rotSpeed);
		}
	}

}
