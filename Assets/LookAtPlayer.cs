using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject playerRef;
    private NavMeshAgent characterRef;

    private void Start()
    {
        playerRef = FindObjectOfType<PlayerController>().gameObject;
        characterRef = GetComponentInParent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (characterRef.desiredVelocity == new Vector3(0, 0, 0))
        {
            transform.LookAt(playerRef.transform.position + new Vector3(0, 85.0F, 0));
        } 
        else
        {
            transform.localRotation = new Quaternion(0, -180, 0, 0);
        }
    }
}
