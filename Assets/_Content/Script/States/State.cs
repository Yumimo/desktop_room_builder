using UnityEngine;

public abstract class State
{
    protected PetController pet;
    private bool allowImmediateExit = false;

    public State(PetController pet, bool allowImmediateExit = false)
    {
        this.pet = pet;
        this.allowImmediateExit = allowImmediateExit;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    public virtual bool CanExit()
    {
        if (allowImmediateExit)
            return true;

        AnimatorStateInfo stateInfo = pet.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !pet.animator.IsInTransition(0);
    }
}