using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
	
	public enum SpawnType{
		Size_SmallToBig,
		Size_BigToSmall
	}
	
	public enum DamageType{
		Fire,
		Ice,
		Electro,
		Normal,
		Count
	}
	
	public enum SpellType{
		Ball,
		Bomb,
		Sparkle,
		Storm,
		Count
	}
	
	public enum MinionType{
		Meele,
		Ranged,
		Tank,
		Healer
	}
}
