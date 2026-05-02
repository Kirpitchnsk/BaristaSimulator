using SibGameJam2026.Services;
using SibGameJam2026.Cameras;
using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public class InputCharacterComponent : IInputCharacterComponent {
		private readonly ACharacter _character;
		public ACharacter Character => _character;
		private readonly IInputService _inputService;
		private readonly ICameraService _cameraService;

		private const string MovementAction = "Movement";
		private const string InteractAction = "Interact";
		private const float MoveDeadZone = 0.05f;

		public InputCharacterComponent(ACharacter character, IInputService inputService, ICameraService cameraService) {
			_character = character;
			_inputService = inputService;
			_cameraService = cameraService;
		}

		public void OnUpdate() {
			if (_inputService.WasButtonPressedThisFrame(InteractAction) &&
			    _character.TryGetComponent<IInteractableComponent>(out var interactableComponent)) {
				interactableComponent.Interact();
			}

			if (!_character.TryGetComponent<IMovementCharacterComponent>(out var movementComponent)) {
				return;
			}

			var moveInput = _inputService.GetVector(MovementAction);
			var hasActiveCamera = _cameraService.TryGetActiveCamera(out var activeCamera);

			Vector3 forward;
			Vector3 right;

			if (hasActiveCamera) {
				forward = activeCamera.transform.forward;
				right = activeCamera.transform.right;
			} else {
				forward = _character.transform.forward;
				right = _character.transform.right;
			}

			forward.y = 0f;
			right.y = 0f;
			forward.Normalize();
			right.Normalize();

			var moveDirection = forward * moveInput.y + right * moveInput.x;

			if (moveDirection.sqrMagnitude > MoveDeadZone * MoveDeadZone) {
				movementComponent.SetMoveInput(moveDirection);
			} else {
				movementComponent.Stop();
			}
		}
	}
}
