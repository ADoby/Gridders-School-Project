using UnityEngine;
using System.Collections;

public class FloatingBlock : MonoBehaviour {

    public Vector3 minPositionDiff;
    public Vector3 maxPositionDiff;

    public float directionChangeTime = 2.0f;
    public float speed = 1.0f;

    private float directionChangeTimer = 0f;

    private Vector3 defaultPosition = Vector3.zero;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 wantedPosition = Vector3.zero;

    private const float PI = 3.1415f;


	// Use this for initialization
	void Start () {
        defaultPosition = transform.position;
        startPosition = defaultPosition;
        wantedPosition = defaultPosition;

        StartCoroutine(UpdateWantedPosition());
	}

    IEnumerator UpdateWantedPosition()
    {
        startPosition = transform.position;

        wantedPosition = defaultPosition;
        wantedPosition.x += Random.Range(minPositionDiff.x, maxPositionDiff.x);
        wantedPosition.y += Random.Range(minPositionDiff.y, maxPositionDiff.y);
        wantedPosition.z += Random.Range(minPositionDiff.z, maxPositionDiff.z);

        directionChangeTimer = 0;

        yield return new WaitForSeconds(directionChangeTime);
        StartCoroutine(UpdateWantedPosition());
    }

	// Update is called once per frame
	void Update () {
        directionChangeTimer += Time.deltaTime;

        directionChangeTimer = Mathf.Clamp(directionChangeTimer, 0, directionChangeTime);

        float multSpeed = Mathf.Clamp(directionChangeTimer*2f, 0, directionChangeTime) / directionChangeTime; //0 => 1

        transform.position = Vector3.Slerp(transform.position, wantedPosition, Time.deltaTime * speed * multSpeed);
	}
}
