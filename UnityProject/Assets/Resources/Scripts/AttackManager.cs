using UnityEngine;
using System.Collections;

public class AttackManager : MonoBehaviour {

    public string Spell1 = "Spell1";

    public Transform shootPosition;
    public Transform targetPosition;

    public bool towardsMouse = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Screen.lockCursor && Input.GetMouseButtonDown(0))
        {
            

            GameObject go = GameObjectPool.Instance.Spawn(Spell1, shootPosition.position, shootPosition.rotation);
            Blast blast = go.GetComponent<Blast>();
            blast.reset();
            if (towardsMouse)
            {
                blast.SetDirection((targetPosition.position - shootPosition.position).normalized);
            }
            else
            {
                blast.SetDirection(shootPosition.forward);
            }
            blast.poolName = Spell1;
            blast.SetDamage(10);
            
        }
	}
}
