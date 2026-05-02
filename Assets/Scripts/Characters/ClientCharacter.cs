using SibGameJam2026;
using SibGameJam2026.Characters;
using SibGameJam2026.Characters.Components;
using UnityEngine;

public class ClientCharacter : SimpleCharacter, IInteractable {
	public void OnInteract(InteractContext context) {
		if (TryGetComponent<IInteractableCharacterComponent>(out var interactable))
			interactable.OnInteract(context);
	}
}
