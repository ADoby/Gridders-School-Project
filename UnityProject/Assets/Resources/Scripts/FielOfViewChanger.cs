using UnityEngine;
using System.Collections;

public class FielOfViewChanger : MonoBehaviour {

    public float startFOV = 90;
    public float zoomedFOV = 40;
    public float changeSpeed = 2.0f;

    private DepthOfFieldScatter dofScript;

    private float startFocalSize;
    public float zoomedFocalSize;
	
    void Start(){
        dofScript = GetComponent<DepthOfFieldScatter>();
        startFocalSize = GetComponent<DepthOfFieldScatter>().focalSize;
    }

	// Update is called once per frame
	void Update () {
        float wantedFOV = startFOV;
        float wantedFocalSize = startFocalSize;

        if (Input.GetMouseButton(1))
        {
            wantedFOV = zoomedFOV;
            wantedFocalSize = zoomedFocalSize;
        }

        dofScript.focalSize = Mathf.Lerp(dofScript.focalSize, wantedFocalSize, Time.deltaTime * changeSpeed);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, wantedFOV, Time.deltaTime * changeSpeed);

	}
}
