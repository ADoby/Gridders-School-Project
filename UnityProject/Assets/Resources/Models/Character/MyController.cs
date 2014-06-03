using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class MyController : MonoBehaviour
{
    private Animator Actor;                                           // The Animator component attached.
    //
    public Transform leftHip;
    public Transform Body;
    public float hitDistance;

    void Start()
    {
        Actor = GetComponent<Animator>();
    }
    //
    void OnAnimatorIK(int Layer)
    {
        if (Actor.enabled)
        {

            RaycastHit hitLeft;
            if (Physics.Raycast(leftHip.position, Vector3.down, out hitLeft))
            {
                hitDistance = hitLeft.distance;
                if (hitLeft.distance < 0.5f)
                {
                    Body.position = Vector3.Lerp(Body.position, Body.position + Vector3.up, Time.deltaTime);
                }
                Actor.SetIKPosition(AvatarIKGoal.LeftFoot, hitLeft.point);
                Actor.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            }

            
        }
    }
}
