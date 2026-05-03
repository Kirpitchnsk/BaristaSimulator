using UnityEngine;
using SibGameJam2026;
using SibGameJam2026.Cameras;
using SibGameJam2026.Services;

namespace SibGameJam2026.Characters.Components {
	public class InteractableCharacterComponent : IInteractableComponent {
		private const float InteractDistance = 1.5f;
		private static readonly int ItemLayerMask = LayerMask.GetMask("Item");

		private readonly ACharacter _character;
		private readonly ICameraService _cameraService;
		private readonly IInteractionSoundService _interactionSoundService;

		public ACharacter Character => _character;

		public InteractableCharacterComponent(
			ACharacter character,
			ICameraService cameraService,
			IInteractionSoundService interactionSoundService
		) {
			_character = character;
			_cameraService = cameraService;
			_interactionSoundService = interactionSoundService;
		}

		public void Interact() {
			if (!_cameraService.TryGetActiveCamera(out var controller))
				return;

			var activeCamera = controller.GetComponent<Camera>();
			if (activeCamera == null)
				return;

			var ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (!Physics.Raycast(ray, out var hit, 5, ItemLayerMask))
				return;

			var distanceToItem = Vector3.Distance(activeCamera.transform.position, hit.point);
			if (distanceToItem > InteractDistance)
				return;

			var interactable = hit.collider.GetComponentInParent<IInteractable>();
			if (interactable == null)
				return;

			var usedItem = _character.TryGetComponent<IInventoryComponent>(out var inventoryComponent)
				? inventoryComponent.CurrentItem
				: default;

			if (interactable is ISound sound && sound.InteractionSound != ESoundType.None) {
				var soundHost = (interactable as Component)?.gameObject ?? _character.gameObject;
				_interactionSoundService.Play(soundHost, sound.InteractionSound);
			}

			interactable.OnInteract(new InteractContext(_character, usedItem));
		}
	}
}
