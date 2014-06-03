using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MinionSpawner : MonoBehaviour {

    private static MinionSpawner inst;
	

    public static MinionSpawner Get()
    {
        if (!inst)
        {
            inst = (MinionSpawner)FindObjectOfType(typeof(MinionSpawner));

            if (inst)
            {
                //Debug.Log("Es darf nur eine instanz dieser klasse live sein!");
            }
        }
        return inst;
    }


	private bool spawning;
	public Transform minionSpawnPosition;	
	private Transform spawningMinion;
	public Transform[] minions;

	
	private float spawnTimer;
	public float spawnTime;	
	public GameObject minion;

    public Transform plane;
	public float distanceToPlayer;
	
	
	public Transform minionContainer;
	
	int id;
	// Use this for initialization
	void Start () 
	{
		//SpawnMinions();
       // spawnEnemyMinions();
		//spawnEnemyMinionsOnPos(minionSpawnPosition, 4, Constants.MinionType.Meele);
	}
	
	// Update is called once per frame
	void Update () 
	{		
		spawnTimer += Time.deltaTime;
		if(spawnTimer >= spawnTime)
		{
			spawnTimer = 0;
			//spawnEnemyMinions();

            spawnEnemyMinionsOnPlane(plane, 4, Constants.MinionType.Meele);
            //spawnEnemyMinionsOnPos(minionSpawnPosition, 4, Constants.MinionType.Meele);
		}
	}	
	
	public void spawnEnemyMinions()
	{
		Vector3 spawnPos = Random.rotation * (transform.position + Vector3.up*distanceToPlayer);
		spawnPos.y = 0;
		
		GameObject go = (GameObject)Instantiate (minion, spawnPos, Random.rotation);
		go.name = minion.name + ":" + id;
		go.GetComponent<MinionKI>().target = transform;
		go.transform.parent = minionContainer;
		id++;		
	}
	
	public void spawnEnemyMinionsOnPos(Transform minionSpawnPosition, int minionCount, Constants.MinionType minionType)
	{
		if (minionType == Constants.MinionType.Meele) 
		{
			for (int i = 0; i < minionCount; i++) 
			{					
				Vector3 spawnPos = minionSpawnPosition.position;	
				
				GameObject go = (GameObject)Instantiate (minion, spawnPos, Random.rotation);
				go.name = minion.name + ":" + id;
				go.GetComponent<MinionKI>().target = transform;
				go.transform.parent = minionContainer;
				id++;	
			}
		}
	}
	
	public void spawnEnemyMinionsOnPlane(Transform plane, int minionCount, Constants.MinionType minionType)
	{		
		
		if (minionType == Constants.MinionType.Meele) 
		{			
			for (int i = 0; i < minionCount; i++) 
			{									
                float randX = Random.Range(-plane.renderer.bounds.size.x / 2, 
                                                plane.renderer.bounds.size.x / 2);

                float randY = Random.Range(-plane.renderer.bounds.size.y / 2, 
                                                plane.renderer.bounds.size.y / 2);

                float x = plane.transform.position.x + randX;
                float y = plane.transform.position.y + randY;
                float z = plane.transform.position.z;
              
                Vector3 spawnPos = new Vector3(x,y,z);

				GameObject go = (GameObject)Instantiate (minion, spawnPos, Random.rotation);
				go.name = minion.name + ":" + id;
				go.GetComponent<MinionKI>().target = transform;
				go.transform.parent = minionContainer;
				id++;	
			}
		}
	}
	/*
	private void SpawnMinions()
	{
		//Wants to Spawn something
		if(!spawning && Input.GetButtonDown ("1")){
			spawningMinion = (Transform) Instantiate(minions[(int)MinionKI.MinionType.Meele], minionSpawnPosition.position, transform.rotation);

			spawning = true;
			spawnTime = spawningMinion.GetComponent<MinionKI>().getSpawnTime();
			spawningMinion.localScale = Vector3.zero;
			spawnTimer = spawnTime;
		}else if(!spawning && Input.GetButtonDown ("2")){
			spawningMinion = (Transform) Instantiate(minions[(int)MinionKI.MinionType.Ranged], minionSpawnPosition.position, transform.rotation);

			spawning = true;
			spawnTime = spawningMinion.GetComponent<MinionKI>().getSpawnTime();
			spawningMinion.localScale = Vector3.zero;
			spawnTimer = spawnTime;
		}else if(spawning){
			//Is Spawning something
			spawnTimer -= Time.deltaTime;
			
			spawningMinion.localScale = (new Vector3(1,1,1)/spawnTime * (spawnTime-spawnTimer));
			
			if(spawnTimer <= 0){
				spawningMinion.GetComponent<MinionKI>().setTarget(transform);
				spawningMinion.localScale = new Vector3(1,1,1);
				spawningMinion = null;
				spawning = false;
			}
		}	
	}
	*/
}
