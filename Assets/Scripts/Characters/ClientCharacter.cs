using SibGameJam2026.Characters.Components;

namespace SibGameJam2026.Characters {
	public class ClientCharacter : SimpleCharacter, IInteractable {
		public string InteractItemName => "Клиент";

		public void OnInteract(InteractContext context)
		{
			if (TryGetComponent<IInteractableCharacterComponent>(out var interactable))
				interactable.OnInteract(context);
		}
	}
}