using UnityEngine;
using System.Collections;

public class DOFTarget : MonoBehaviour {

    public Transform target;

    public float range = 100.0f;

    public bool towardsMouse = true;

	// Update is called once per frame
	void Update () {
        Ray ray;
        if (towardsMouse)
        {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            ray = new Ray(transform.position, transform.forward);
        }
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            target.position = hit.point;
        }else{
            target.position = transform.position + ray.direction * range;
        }
	}
}
