using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class Spell
	{
		public Constants.SpellType spellType;
		
		public Spawner spawner;
		
		public float releaseSpeed;
		public float speedOverTime;
		public float speedBoostOnCollide;
		
		public Vector2 minMaxSize;
		public float size;
		public float damage;
		public float speed;
		
		public bool sizeChangesDamage;
		public bool sizeChangesSpeed;
		
		public int sparkleCount;
		public Transform sparkle;
		public bool spawnSparklesOnRelease;
		public bool spawnSparklesOnDetonate;
		public bool spawnSparklesOnImpact;
		
		public bool holdAfterSpawn;
		public bool releaseWithMouse;
		
		private Transform transform;
		
		public bool wantsToBeInIceStorm;
		public bool wantsToBeInFireStorm;
		
		public bool SparklesAreEffect;

		public LayerMask targetLayer;

		public void init(Transform _transform){
			transform = _transform;
			spawner.init(_transform);

			init ();
		}
		
		public void init(Transform _transform, Vector3 _size){
			transform = _transform;
			spawner.init(_transform, _size);
			
			init ();
		}
		
		private void init(){
			randomizeSize();
			transform.localScale = new Vector3(1,1,1) * size;
			spawner.setFinishedSize(transform.localScale);
		}
		
		public bool wantsToBeRotated(){
			return wantsToBeInIceStorm || wantsToBeInFireStorm;	
		}
		
		public float getSpawnTime ()
		{
			return spawner.spawnTime;
		}
		
		public bool updateSpawner(){
			return spawner.update();	
		}

		public void instantSpawn ()
		{
			spawner.instantSpawn();
		}

		public bool properTarget(int layer){
			if(((1<<layer) & targetLayer) != 0)
			{
				return true;
			}else{
				return false;
			}
		}
		
		private void randomizeSize(){
			size = Random.Range(minMaxSize.x,minMaxSize.y);
			if(sizeChangesSpeed) speed = speed / size;
			if(sizeChangesDamage) damage = damage * size;
		}
	}
}

