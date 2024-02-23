using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Vector3 debugPosition;
    private PlayerController playerRef;

    private void Awake()
    {
        playerRef = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();

        //AgentGoTo(playerRef.transform.position);
    }

    void AgentIdle()
    {

    }

    public void AgentGoTo(Vector3 targetPosition) 
    {
        NavMeshPath path = new NavMeshPath();
        //agent.CalculatePath(targetPosition, path);
        //agent.SetPath(path);
        agent.SetDestination(targetPosition);
    }

    void AgentDoAction()
    {

    }

    private void FixedUpdate()
    {
        
    }
}