using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlockThingy{
	public Transform thingy;
	public float speed;
	public int direction = 1;
}

public enum LoopMode 
{
	OneShot,
	PingPong
}

public class GoAwayBlock : MonoBehaviour {
	
	public BlockThingy[] blockThings;
	
	public bool getChildren;
	public bool randomize;
	public float minSpeed;
	public float maxSpeed;
	
	public LoopMode loopMode;
	
	public bool goAway = false;
	
	// Use this for initialization
	void Start () {
		if(getChildren){
			blockThings = new BlockThingy[transform.childCount];
 			int i = 0;
			foreach (Transform child in transform)
			{
				blockThings[i] = new BlockThingy();
				blockThings[i].thingy = child;
				i++;
			}
		}
		if(randomize){
			foreach(BlockThingy thing in blockThings){
				thing.speed = Random.Range(minSpeed, maxSpeed);
			}
		}
		
		if(loopMode == LoopMode.OneShot)
			goAway = true;
	}
	
	// Update is called once per frame
	void Update () {
		foreach(BlockThingy thing in blockThings){
			if(!goAway && thing.direction == 0){
				thing.direction = -1;
			}
			if(loopMode == LoopMode.PingPong){
				if(thing.thingy.localPosition.z + (Vector3.forward * thing.speed * Time.deltaTime * thing.direction).z >= 2 || thing.thingy.localPosition.z + (Vector3.forward * thing.speed * Time.deltaTime * thing.direction).z <= 0){
					if(thing.direction == 1){
						if(!goAway)
							thing.direction = -thing.direction;
						else{
							thing.thingy.localPosition = new Vector3(thing.thingy.localPosition.x, thing.thingy.localPosition.y, 2);
							thing.direction = 0;
						}
					}else if(thing.direction == -1)
						thing.direction = -thing.direction;
				}
			}else if(loopMode == LoopMode.OneShot){
				if(thing.thingy.localPosition.z + (Vector3.forward * thing.speed * Time.deltaTime * thing.direction).z >= 2){
					thing.thingy.localPosition = new Vector3(thing.thingy.localPosition.x, thing.thingy.localPosition.y, 2);
					thing.direction = 0;
				}
			}
			thing.thingy.Translate(Vector3.forward * thing.speed * Time.deltaTime * thing.direction);
		}
	}
}
