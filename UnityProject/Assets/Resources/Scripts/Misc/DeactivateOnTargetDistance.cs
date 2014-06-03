using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeactivateOnTargetDistance : MonoBehaviour {

    public Transform target;
    public float maxDistance;
    public GameObject[] things;
		
	public bool DebugOn = true;

#if UNITY_EDITOR
	void OnRenderObject() {
		if (DebugOn && Camera.current && things.Length != 0)
		{

			if (things[0].activeSelf && Vector3.Distance(Camera.current.transform.position, transform.position) >= maxDistance)
			{
				foreach (GameObject obj in things)
				{
					obj.SetActive(false);
				}
			} if (!things[0].activeSelf && Vector3.Distance(Camera.current.transform.position, transform.position) < maxDistance)
			{
				foreach (GameObject obj in things)
				{
					obj.SetActive(true);
				}
			}
		}
	}
	void Update(){
		GetComponent<LODGroup>().RecalculateBounds();
	}
#endif

	// Use this for initialization
	void Start () {
		target = GameObject.Find("Player").transform;

		if (target && things.Length != 0)
		{
			StartCoroutine(Test());
		}
	}
	
	IEnumerator Test()
	{
		if (things[0].activeSelf && Vector3.Distance(target.position, transform.position) >= maxDistance)
		{
			foreach (GameObject obj in things)
			{
				obj.SetActive(false);
			}
		} if (!things[0].activeSelf && Vector3.Distance(target.position, transform.position) < maxDistance)
		{
			foreach (GameObject obj in things)
			{
				obj.SetActive(true);
			}
		}

		yield return new WaitForSeconds(1.0f);
		StartCoroutine(Test());
	}

}
