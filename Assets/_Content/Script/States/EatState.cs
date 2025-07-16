using UnityEngine;
using UnityEngine.AI;

public class EatingState : State
{
    private NavMeshAgent agent;
    private Vector3 foodPosition;
    private bool hasReachedFood = false;
    private bool animationFinished = false;

    public EatingState(PetController pet, Vector3 foodPosition) : base(pet)
    {
        this.foodPosition = foodPosition;
    }

    public override void Enter()
    {
        agent = pet.agent;
        agent.isStopped = false;
        agent.SetDestination(foodPosition);
        hasReachedFood = false;
        animationFinished = false;
    }

    public override void Update()
    {
        if (!hasReachedFood)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                hasReachedFood = true;

                Vector3 lookDir = (foodPosition - pet.transform.position).normalized;
                lookDir.y = 0;
                if (lookDir != Vector3.zero)
                    pet.transform.rotation = Quaternion.LookRotation(lookDir);

                pet.animator.Play("eat");
            }
        }
        else
        {
            AnimatorStateInfo stateInfo = pet.animator.GetCurrentAnimatorStateInfo(0);
            animationFinished = stateInfo.normalizedTime >= 1f && !pet.animator.IsInTransition(0);
        }
    }

    public override bool CanExit()
    {
        return hasReachedFood && animationFinished;
    }

    public override void Exit()
    {
        agent.ResetPath();
        pet.isHungry = false;
        pet.hungerTimer = 0;
    }
}