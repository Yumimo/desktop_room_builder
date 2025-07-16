public class HungryState : State
{
    public HungryState(PetController pet) : base(pet, true) { }

    public override void Enter()
    {
        pet.agent.isStopped = true;
        pet.animator.Play("hungry");
    }

    public override void Update()
    {
        // Wait for external trigger (like detecting food or player feed)
    }

    public override bool CanExit()
    {
        return false;
    }

    public override void Exit()
    {
        pet.agent.isStopped = false;
    }
}