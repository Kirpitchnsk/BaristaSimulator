using SibGameJam2026.Characters;
using SibGameJam2026.Characters.Components;
using UnityEngine;
using Zenject;

public class PokupatellAnimateControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private INpcControlStateCharacterComponent stateComponent;

    private static readonly int IsAnimating = Animator.StringToHash("IsRun");
    private static readonly int Drink = Animator.StringToHash("Drink");

    [Inject]
    public void Construct(INpcControlStateCharacterComponent stateComponent)
    {
        this.stateComponent = stateComponent;
    }

    private void OnEnable()
    {
        if (stateComponent != null)
            stateComponent.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        if (stateComponent != null)
            stateComponent.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(EClientState state)
    {
        switch (state)
        {
            case EClientState.WalkToOrder:
            case EClientState.Leave:
                animator.SetBool(IsAnimating, true);
                break;

            case EClientState.WaitInteraction:
                animator.SetBool(IsAnimating, false);
                break;

            case EClientState.WaitCooking:
                animator.SetBool(IsAnimating, false);
                animator.SetTrigger(Drink);
                break;

            case EClientState.TransformCreatureSuccess:
            case EClientState.TransformCreatureFailed:
                animator.SetBool(IsAnimating, false);
                break;
        }
    }
}