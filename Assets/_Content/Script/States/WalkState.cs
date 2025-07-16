using UnityEngine;
using UnityEngine.AI;

public class WalkState : State
{
    private NavMeshAgent agent;
    private bool destinationReached;
    private float stuckTimer;
    private Vector3 lastPosition;
    private float checkInterval = 0.5f;
    private float stuckDurationThreshold = 2f;

    public WalkState(PetController pet) : base(pet) { }

    public override void Enter()
    {
        destinationReached = false;
        agent = pet.agent;
        agent.isStopped = false;
        stuckTimer = 0f;
        lastPosition = pet.transform.position;

        SetRandomDestination();
        pet.animator.SetFloat("speed", 0f);
    }

    public override void Update()
    {
        float moveSpeed = agent.velocity.magnitude;
        pet.animator.SetFloat("speed", moveSpeed);

        // Face movement direction
        if (moveSpeed > 0.01f)
        {
            Vector3 lookDirection = agent.velocity.normalized;
            lookDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            pet.transform.rotation = Quaternion.Slerp(pet.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Destination reached normally
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            destinationReached = true;
            return;
        }

        // Handle stuck detection
        stuckTimer += Time.deltaTime;
        if (stuckTimer >= checkInterval)
        {
            float movedDistance = Vector3.Distance(pet.transform.position, lastPosition);
            if (movedDistance < 0.05f && agent.hasPath)
            {
                // Pet hasn't moved much → likely stuck
                Debug.LogWarning("Pet is stuck. Forcing state exit.");
                destinationReached = true;
            }

            lastPosition = pet.transform.position;
            stuckTimer = 0f;
        }

        // Handle broken path
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || !agent.hasPath)
        {
            Debug.LogWarning("Invalid NavMesh path. Forcing fallback.");
            destinationReached = true;
        }
    }

    public override bool CanExit()
    {
        return destinationReached;
    }

    public override void Exit()
    {
        agent.isStopped = true;
        agent.ResetPath();
        pet.RotateTowardCamera();
    }

    private void SetRandomDestination()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += pet.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(pet.transform.position, hit.position, NavMesh.AllAreas, path)
                    && path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        Debug.LogWarning("Failed to find valid walk destination after 10 attempts.");
        destinationReached = true;
    }
}
