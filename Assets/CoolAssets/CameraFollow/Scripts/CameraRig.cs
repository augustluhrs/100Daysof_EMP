using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{

    enum UpdateCycle
    {
        NORMAL, FIXED, LATE

    }
    [SerializeField] Transform targetTransform;
    [SerializeField] Rigidbody targetRigidbody;
    [SerializeField] Transform cameraPivot;

    [Space]
    [Header("Follow Settings")]

    [Tooltip("The closer to '1', the faster the camera will follow")]
    [Range(0.0001f, 1f)]
    [SerializeField] float positionFollowIntensity = 0.5f;

    [Tooltip("The closer to '1', the faster the camera will follow")]
    [Range(0.0001f, 1f)]
    [SerializeField] float rotationFollowIntensity = 0.5f;
    [Tooltip("Velocity at which camera will start to follow target")]
    [SerializeField] float minimumVelocityToFollow = 0.1f;

    public Quaternion staticDirection;

    [Space]
    [Header("Physics")]
    [Tooltip("Adjust this setting if camera is stuttering. Usually camera stutters are the result of an update cycle that is out of sync with physics.")]
    [SerializeField] UpdateCycle updateCycle = UpdateCycle.NORMAL;

    [Space]
    [SerializeField] public Vector3 velocityFollowModifier = Vector3.one;
    Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        if (updateCycle == UpdateCycle.NORMAL)
        {
            FollowTargetPosition();
            RotateTowardsTargetVelocity();
        }
    }

    void FixedUpdate()
    {
        if (updateCycle == UpdateCycle.FIXED)
        {
            FollowTargetPosition();
            RotateTowardsTargetVelocity();
        }
    }

    void LateUpdate()
    {
        if (updateCycle == UpdateCycle.LATE)
        {
            FollowTargetPosition();
            RotateTowardsTargetVelocity();
        }
    }

    void FollowTargetPosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, positionFollowIntensity);
    }

    void RotateTowardsTargetVelocity()
    {
        
        //not sure how to do this for multiple people, will prob need some sort of offset on the client end that they can calibrate manually with a button
        if(staticDirection != null){
            cameraPivot.localRotation = staticDirection; //to look down?
            // Debug.Log("allalala" + staticDirection.eulerAngles);
        }

        // if (targetRigidbody.velocity.magnitude > minimumVelocityToFollow)
        // {
        //     Vector3 direction = targetRigidbody.velocity;
        //     Vector3 vm = velocityFollowModifier; //shorthand
        //     direction = new Vector3 (direction.x * vm.x, direction.y * vm.y, velocityFollowModifier.z * vm.z);  // There's a better way to do this but my brain is goop rn.
        //     Quaternion directionAsQuat = Quaternion.LookRotation(direction);
        //     cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, directionAsQuat, rotationFollowIntensity);
        // }
        
    }
}
