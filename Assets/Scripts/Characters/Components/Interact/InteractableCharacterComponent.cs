using UnityEngine;
using SibGameJam2026.Cameras;

namespace SibGameJam2026.Characters.Components {
	public class InteractableCharacterComponent : IInteractableComponent {
		private const float InteractDistance = 1.5f;
		private static readonly int ItemLayerMask = LayerMask.GetMask("Item");

		private readonly ACharacter _character;
		private readonly ICameraService _cameraService;
		public ACharacter Character => _character;

		public InteractableCharacterComponent(ACharacter character, ICameraService cameraService) {
			_character = character;
			_cameraService = cameraService;
		}

		public void Interact() {
			if (!_cameraService.TryGetActiveCamera(out var activeCamera))
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

			interactable.OnInteract(new InteractContext(_character, usedItem));
		}
	}
}
