using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerController : TargetAble {
	
	enum PlayerMode{
		Casting,
		Spawning
	}


	public Animator[] animators;
	public CharacterController []charControls;
	public RayCastPosition []cameraSripts;
	public PlayerAttackManager []attackScripts;

	private int currentChosenPuppet = 0;

	public Transform[] minions;
	
	public Transform[] spells;
	
	public float speed;
	public float sprintMultiply;
	public float jumpHeight;
	public bool canJump;
	public float gravityMultiply;
	
	public Vector3 currentVelocity;
	
	public bool grounded;
	
	public float rotationSpeed;
	
	private int rotationMask;
	public float rotationRayLength;
	
	public bool moveForward;
	public bool arrowsToRotate;
	public bool FPS;
	
	//Spawn Minions Variables
	
	private bool spawning = false;
	//private float spawnTimer;
	//private float spawnTime;
	//public Transform minionSpawnPosition;
	
	//private Transform spawningMinion;
	//Ende
	
	//casting Variabless
	private bool casting, holding;
	private float castTimer,castTime;
	public Transform spellSpawnPosition;
	
	private Transform castingSpell = null;
	
	public Transform SpellContainer;
	
	private int spellControlMask;
	//Ende
	
	private PlayerMode currentMode;
	
	public float speedMultWhenCasting;
	
	public Transform iceStormContainer;
	public Transform fireStormContainer;
	
	public float iceStormRotateSpeed;
	public float fireStormRotateSpeed;
	
	public Transform SpellHoldPosition;
	
	public float rotationWithMouse;

    public bool jumping = false;
    public TriggerBool foodSensor;
    public TriggerBool headSensor;

    public Transform player;
    public float RotateX, RotateY;
    public float RotationDamping = 3.0f;

	private float maxHealth = 1.0f;

	public Healthbar healthbar;
	public string DeathEffektPoolName = "";
	
	private float currentSpeed;
	public float speedDamping;

	public override void doDamage(float damage, string damageColor){
		base.doDamage(damage, damageColor);
		//healthbar.UpdateHealth(health/maxHealth);
		//TODO Update HealthBar / Event

		if(isDestroyed()){
			//GameObjectPool.Instance.Despawn(poolName, gameObject);
			
			if(DeathEffektPoolName != ""){
				GameObject go = GameObjectPool.Instance.Spawn(DeathEffektPoolName, transform.position, Quaternion.identity);
				if(go.particleSystem)
					go.particleSystem.Play ();
				go.GetComponent<MinionDeathExplosion>().reset ();
			}
			
		}
	}

	// Use this for initialization
	void Start () {
		//charControl = GetComponent<CharacterController>();
		maxHealth = health;
						 //Floor
		rotationMask = 1 << 8;
		
		spellControlMask = 1 << 10 | 1 << 11; //ignore Enemies (So we can hit them)
		spellControlMask = ~spellControlMask;
		
		currentMode = PlayerMode.Casting;


		deactivateAllCameraScripts();
		deactivateAllAttackScripts();
		currentChosenPuppet = 0;
		cameraSripts[0].enabled = true;
		attackScripts[0].enabled = true;
}

	public void deactivateAllCameraScripts()
	{
		cameraSripts[0].enabled = false;
		cameraSripts[1].enabled = false;
		cameraSripts[2].enabled = false;
		cameraSripts[3].enabled = false;
	}

	public void deactivateAllAttackScripts()
	{
		attackScripts[0].enabled = false;
		attackScripts[1].enabled = false;
		attackScripts[2].enabled = false;
		attackScripts[3].enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		

		if(FPS){
			if(!Screen.lockCursor && Input.GetMouseButtonDown(0))
			{
				//Lock Cursor
				Screen.lockCursor = true;
			}else if(Screen.lockCursor && Input.GetButtonDown ("ESC"))
			{
				Screen.lockCursor = false;
			}
		}
        
		//Movement only if not doing something
		float wantedSpeed = Input.GetButton("Sprint") ? (sprintMultiply * speed) : speed;
		wantedSpeed = casting|spawning ? speed * speedMultWhenCasting : wantedSpeed;

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		    currentSpeed = Vector2.Lerp (new Vector2(currentSpeed,0),new Vector2(wantedSpeed,0), Time.deltaTime * speedDamping).x;
		
		currentVelocity.z = Input.GetAxis ("Vertical") * currentSpeed;
		
		if(!arrowsToRotate)
		{
			currentVelocity.x = Input.GetAxis ("Horizontal") * currentSpeed;
			if(FPS){
				charControls[currentChosenPuppet].transform.Rotate (Vector3.up * rotationSpeed * Input.GetAxis("Mouse X"));
			}else{
				
				
				//Rotation to Mouse
				RaycastHit hit;
				
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, rotationRayLength, rotationMask)){
					if(hit.collider.gameObject.tag == "Floor"){
						Quaternion rotation = Quaternion.LookRotation(hit.point - charControls[currentChosenPuppet].transform.position);
						rotation.x = transform.rotation.x;
						rotation.z = transform.rotation.z;
						charControls[currentChosenPuppet].transform.rotation = Quaternion.Lerp(charControls[currentChosenPuppet].transform.rotation, rotation, Time.deltaTime * rotationSpeed);
					}
				}
			}
		}else{

			charControls[currentChosenPuppet].transform.Rotate (Vector3.up * rotationWithMouse * Input.GetAxis("Mouse X"));	
			currentVelocity.x = Input.GetAxis ("Horizontal") * currentSpeed;
		}

        if (jumping && headSensor.Hitting())
        {
            //Fall down
            currentVelocity.y = Mathf.Clamp(-currentVelocity.y, 0, -1.0f);
            jumping = false;
        }

        if (jumping && currentVelocity.y <= 0)
            jumping = false;

        if(!jumping)
        {
            if (foodSensor.Hitting() && Input.GetButtonDown("Jump"))
            {
                currentVelocity.y = computeJumpPower();
                jumping = true;
            }
		}else{
            currentVelocity.y += Physics.gravity.y * Time.deltaTime * gravityMultiply;
		}


		charControls[currentChosenPuppet].Move (charControls[currentChosenPuppet].transform.TransformDirection(currentVelocity) * Time.deltaTime);

		//Rotate Player thing
        //player.localRotation = Quaternion.Lerp(player.localRotation, Quaternion.Euler(Vector3.right * RotateX * currentVelocity.z + Vector3.forward * RotateY * currentVelocity.x), Time.deltaTime * RotationDamping);
		
		if(currentMode == PlayerMode.Casting){
//			Casting();
		}else if(currentMode == PlayerMode.Spawning){
			//SpawnMinions();
		}
		
//TODO		animators[currentChosenPuppet].speed = currentVelocity.z;
	}


    public float FloatingHeight = 0.5f;
    public float FloatingSpeed = 2.0f;
    public float MaxFloatingSpeed = 5.0f;
    private bool hardLanding = false;
    public LayerMask groundCheckMask;

	private float computeJumpPower(){
		return (jumpHeight * -Physics.gravity.y)/3;
	}	
	
	public void attack(float wfejpo){}

}
