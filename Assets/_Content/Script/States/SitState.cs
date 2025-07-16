using UnityEngine;

public class SitState : State
{
    public SitState(PetController pet) : base(pet) { }

    public override void Enter()
    {
        pet.animator.Play("sit");
    }
}
