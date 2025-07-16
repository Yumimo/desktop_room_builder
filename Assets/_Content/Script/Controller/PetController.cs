using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PetController : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;

    [Header("State Timings")]
    public float stateDurationMin = 3f;
    public float stateDurationMax = 6f;

    [Header("Hunger Timings")]
    public float hungerDuration = 60;

    [Header("Components")]
    public NavMeshAgent agent;

    [Header("State Weights")]
    public StateWeight[] stateWeights;

    private State currentState;
    private float stateTimer;
    public float hungerTimer;
    public bool isHungry;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        SwitchState(new IdleState(this));
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        hungerTimer += Time.deltaTime;

        currentState?.Update();

        // Trigger hunger
        if (!isHungry && hungerTimer > hungerDuration)
        {
            isHungry = true;
            SwitchState(new HungryState(this));
            return;
        }

        if (stateTimer <= 0 && !(currentState is HungryState))
        {
            if (currentState != null && currentState.CanExit() && IsAnimationDone())
            {
                SwitchRandomState();
            }
        }
    }

    bool IsAnimationDone()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
        stateTimer = Random.Range(stateDurationMin, stateDurationMax);
    }

    private void SwitchRandomState()
    {
        StateType selectedType = GetWeightedRandomState();
        State newState = null;

        switch (selectedType)
        {
            case StateType.Idle:
                if (!(currentState is IdleState))
                    newState = new IdleState(this);
                break;
            case StateType.Walk:
                if (!(currentState is WalkState))
                    newState = new WalkState(this);
                break;
            case StateType.Sit:
                if (!(currentState is SitState))
                    newState = new SitState(this);
                break;
        }

        if (newState == null)
            newState = new IdleState(this);

        SwitchState(newState);
    }

    private StateType GetWeightedRandomState()
    {
        var totalWeight = 0f;

        foreach (var sw in stateWeights)
            totalWeight += sw.weight;

        var rand = Random.Range(0f, totalWeight);
        var cumulative = 0f;

        foreach (var sw in stateWeights)
        {
            cumulative += sw.weight;
            if (rand <= cumulative)
                return sw.type;
        }

        return stateWeights.Length > 0 ? stateWeights[0].type : StateType.Idle;
    }

    public void RotateTowardCamera()
    {
        // Flip the camera direction
        Vector3 camForward = -Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        if (camForward == Vector3.zero)
            return;

        Quaternion targetRot = Quaternion.LookRotation(camForward);

        // Smooth Y-axis rotation
        LeanTween.cancel(gameObject);
        LeanTween.rotateY(gameObject, targetRot.eulerAngles.y, 0.3f).setEase(LeanTweenType.easeOutSine);
    }

    
    public void Feed()
    {
        isHungry = false;
        hungerTimer = 0;
        SwitchRandomState();
    }
}
