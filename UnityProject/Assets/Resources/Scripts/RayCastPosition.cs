using UnityEngine;
using System.Collections;

public class RayCastPosition : MonoBehaviour {

    public float distanceChangeScrollWheel;
    public float distanceChangeDamping;
    private float distance;
    private float wantedDistance;

    public float maxDistance;
    public float minDistance;

    public Transform cam;

    public LayerMask mask;

    public float CameraMoveSpeed;
    private Vector3 wantedCameraPosition;
    public float multiplyWhenSomethingInWay;

    public float rotateSpeedX;
    public float currentX;
    public float maxX, minX;

	// Use this for initialization
	void Start () {
        distance = (maxDistance - minDistance) / 2.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{        
    	currentX += Input.GetAxis("Mouse Y") * rotateSpeedX;

		currentX = Mathf.Clamp(currentX, minX, maxX);

        wantedDistance -= Input.GetAxis("Mouse ScrollWheel") * distanceChangeScrollWheel;
		wantedDistance = Mathf.Clamp (wantedDistance, minDistance, maxDistance);

        distance = wantedDistance;

        Quaternion rotate = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(currentX, 0, 0));
        Debug.DrawLine(transform.position, transform.position + rotate * transform.up);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, rotate*transform.up, out hit, distance, mask))
        {
            wantedCameraPosition = hit.point - (hit.point - transform.position).normalized * 0.5f;
            cam.position = Vector3.Lerp(cam.position, wantedCameraPosition, Time.deltaTime * CameraMoveSpeed * multiplyWhenSomethingInWay);
        }
        else
        {
            wantedCameraPosition = transform.position + rotate*transform.up * distance;
            cam.position = Vector3.Lerp(cam.position, wantedCameraPosition, Time.deltaTime * CameraMoveSpeed);
        }       
	}
}
