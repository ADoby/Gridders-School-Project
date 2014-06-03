using UnityEngine;
using System.Collections;

public class MinionKI : MonoBehaviour {
	
	public enum MinionType{
		Meele,
		Ranged,
		Tank,
		Healer
	}
	
	public bool enemy;
	
	public MinionType minionType;
	
	public float spawnTimer;
	
	private NavMeshAgent agent;
	
	public float minDistanceToPlayer;
	
	public bool follow;

	public Transform target;
	
	private float attackTimer;
	public float attackCooldown;
	
	public Vector2 minMaxSpeed;
	
	public GameObject player;
	
	public ParticleSystem particlefx;
	public GameObject body;
	private float particletime;
	
	// Use this for initialization
	void Start () {
        
		player = GameObject.Find("Player");
		agent = GetComponent<NavMeshAgent>();
		agent.speed = Random.Range (minMaxSpeed.x, minMaxSpeed.y);

        particlefx = this.gameObject.GetComponentInChildren<ParticleSystem>();
		particletime = particlefx.duration + particlefx.startLifetime;        
	}
   
	private bool pCountDown = false;
	// Update is called once per frame
	void Update () 
	{
        
		if(target==null)
		{
			//Stand around
			return;
		}
		
		attackTimer -= Time.deltaTime;
		
		if(agent && agent.enabled && follow)
		{			
			agent.stoppingDistance = minDistanceToPlayer*0.8f;
			agent.destination = target.position;
			
			//Try if we are close enough, attack
			if ((target.position-transform.position).magnitude <= minDistanceToPlayer && attackTimer <= 0){
				attackTimer = attackCooldown;
				target.GetComponent<PlayerController>().attack(1);
			}			
		}		
		
		if (pCountDown) 
		{			
			particletime -=Time.deltaTime;
			if (particletime <= 0) Destroy(this.gameObject);	// Que
		}
		

	}
	void LateUpdate()
	{
		
	}
	
	public float getSpawnTime()
	{
		return spawnTimer;
	}
	
	public void setTarget(Transform newTarget)
	{
		target = newTarget;
	}
	
	void OnCollisionEnter(Collision info)
	{
		//Debug.Log(info.collider.gameObject.name);

        target = null;
		pCountDown = true;
		body.SetActive(false);
		gameObject.collider.enabled = false;
        
		particlefx.Play();
		
	}
	
	/*
	private GameObject FindChild(string pRoot, string pName)	
	{	
	    Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) {
	
	        if (trs.gameObject.name == pName)
	
	            return trs.gameObject;
	
	    }
	
	   return null;
	
	}*/
}
