using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public class MovementCharacterComponent : IMovementCharacterComponent {
		private readonly ACharacter _character;
		private readonly Transform _transform;
		private readonly CharacterController _characterController;
		private Vector3 _moveInput;
		private Vector3 _lookDirection;
		private float _nextDebugLogTime;

		public float MoveSpeed { get; }
		public float RotationSpeed { get; }
		public ACharacter Character => _character;


		public MovementCharacterComponent(ACharacter character, CharacterEntry entry) {
			_character = character;
			_transform = character.transform;
			_characterController = character.Data.CharacterController;
			MoveSpeed = entry.MoveSpeed;
			RotationSpeed = entry.RotationSpeed;
		}

		public void SetMoveInput(Vector3 moveInput) {
			_moveInput = Vector3.ClampMagnitude(moveInput, 1f);
		}

		public void SetLookDirection(Vector3 lookDirection) {
			lookDirection.y = 0f;
			_lookDirection = lookDirection.normalized;
		}

		public void Stop() {
			_moveInput = Vector3.zero;
		}

		public void OnUpdate() {
			if (_moveInput.sqrMagnitude > 0f) {
				var motion = _moveInput * (MoveSpeed * Time.deltaTime);
				if (_characterController != null)
					_characterController.Move(motion);
			}

			if (_lookDirection.sqrMagnitude > 0f) {
				var targetRotation = Quaternion.LookRotation(_lookDirection, Vector3.up);
				_transform.rotation = Quaternion.RotateTowards(
					_transform.rotation,
					targetRotation,
					RotationSpeed * Time.deltaTime);
			}
		}
	}
}
