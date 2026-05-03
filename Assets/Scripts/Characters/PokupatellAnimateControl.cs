using UnityEngine;
using SibGameJam2026.Characters;
using SibGameJam2026.Characters.Components;

public class PokupatellAnimateControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private INpcControlStateCharacterComponent stateComponent;

    private static readonly int IsAnimating = Animator.StringToHash("IsRun");
    private static readonly int Drink = Animator.StringToHash("Drink");

    public void Init(ACharacter character)
    {
        stateComponent = character.GetComponent<INpcControlStateCharacterComponent>();
        stateComponent.StateChanged += OnStateChanged;
    }

    private void OnDestroy()
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

            default:
                animator.SetBool(IsAnimating, false);
                break;
        }
    }
}