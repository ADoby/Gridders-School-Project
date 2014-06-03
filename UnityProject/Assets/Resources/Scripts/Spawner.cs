using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class Spawner
	{
		
		public float spawnTime;
		private float spawnTimer;
		
		public Constants.SpawnType type;
		
		private Vector3 size;
		
		private Transform transform;
		
		public void init(Transform _transform)
		{
			transform = _transform;
			spawnTimer = 0.0f;
		}
		
		public void init(Transform _transform, Vector3 _size){
			transform = _transform;
			size = _size;
		}
		
		public void setFinishedSize(Vector3 _size){
			size = _size;	
		}
		
		//Return Type: 0:Nothing|1:Finished|2:Error
		public bool update(){
			spawnTimer += Time.deltaTime;
			
			if(spawnTimer >= spawnTime){
				updateSize (1);
				return true; //Finished
			}else{
				updateSize (spawnTimer/spawnTime);
			}
			return false; //Spawning
		}
		
		private void updateSize(float progress){
			if(transform){
				if(type == Constants.SpawnType.Size_BigToSmall){
					transform.localScale = size - (size * progress);
				}else if(type == Constants.SpawnType.Size_SmallToBig){
					transform.localScale = size * progress;
				}
			}
		}
		
		public void instantSpawn(){
			updateSize(1);
		}
	}
}

