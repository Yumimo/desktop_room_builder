using UnityEngine;

public class IdleState : State
{
    private string chosenIdleAnim;

    public IdleState(PetController pet) : base(pet) { }

    public override void Enter()
    {
        pet.agent.isStopped = true;
        pet.animator.SetFloat("speed", 0f);

        chosenIdleAnim = PickRandomIdle();
        pet.animator.SetTrigger(chosenIdleAnim);
    }

    public override void Exit()
    {
        // Optional: reset trigger if needed
        pet.animator.ResetTrigger(chosenIdleAnim);
    }

    private string PickRandomIdle()
    {
        string[] idleAnimations = { "idle","waving"};
        int index = Random.Range(0, idleAnimations.Length);
        return idleAnimations[index];
    }
}