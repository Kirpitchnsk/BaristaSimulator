using SibGameJam2026;

namespace SibGameJam2026.Characters.Components {
	/// <summary>
	/// Обработка взаимодействия игрока с NPC (например <see cref="ClientCharacter"/>).
	/// </summary>
	public interface IInteractableCharacterComponent : ICharacterComponent {
		void OnInteract(InteractContext context);
	}
}
