using Arenar.AudioSystem;
using UnityEngine;

namespace SibGameJam2026 {
	public interface ISound {
		ESoundType InteractionSound { get; }
	}

	public interface IInteractable {
		string InteractItemName { get; }

		void OnInteract(InteractContext context);
	}

	public abstract class AInteractItemVisual : MonoBehaviour, IInteractable, ISound {
		[SerializeField] protected ESoundType _interactionSound = ESoundType.None;

		public ESoundType InteractionSound => _interactionSound;

		public abstract string InteractItemName { get; }

		public abstract void OnInteract(InteractContext context);
	}
}