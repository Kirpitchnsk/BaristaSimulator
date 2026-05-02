using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	/// <summary>
	/// Движение без камеры: поворот только по горизонтали (yaw), крен и тангаж не накапливаются.
	/// При движении смотрит в сторону перемещения; стоя — по <see cref="SetLookDirection"/>.
	/// </summary>
	public class NpcMovementCharacterComponent : IMovementCharacterComponent {
		private const float DirectionEpsilon = 1e-6f;

		private readonly ACharacter _character;
		private readonly Transform _transform;
		private readonly CharacterController _characterController;
		private Vector3 _moveInput;
		private Vector3 _lookDirection;

		public float MoveSpeed { get; }
		public float RotationSpeed { get; }
		public ACharacter Character => _character;

		public NpcMovementCharacterComponent(ACharacter character, CharacterEntry entry) {
			_character = character;
			_transform = character.transform;
			_characterController = character.Data.CharacterController;
			MoveSpeed = entry.MoveSpeed;
			RotationSpeed = entry.RotationSpeed;
		}

		public void SetMoveInput(Vector3 moveInput) {
			moveInput.y = 0f;
			_moveInput = Vector3.ClampMagnitude(moveInput, 1f);
		}

		public void SetLookDirection(Vector3 lookDirection) {
			lookDirection.y = 0f;
			_lookDirection = lookDirection.sqrMagnitude > DirectionEpsilon ? lookDirection.normalized : Vector3.zero;
		}

		public void Stop() {
			_moveInput = Vector3.zero;
		}

		public void OnUpdate() {
			if (_moveInput.sqrMagnitude > DirectionEpsilon) {
				var motion = _moveInput * (MoveSpeed * Time.deltaTime);
				if (_characterController != null)
					_characterController.Move(motion);
			}

			var moveDir = _moveInput.sqrMagnitude > DirectionEpsilon ? _moveInput.normalized : Vector3.zero;
			var faceDir = moveDir.sqrMagnitude > DirectionEpsilon ? moveDir : _lookDirection;

			if (faceDir.sqrMagnitude <= DirectionEpsilon)
				return;

			var targetYaw = Mathf.Atan2(faceDir.x, faceDir.z) * Mathf.Rad2Deg;
			var newYaw = Mathf.MoveTowardsAngle(_transform.eulerAngles.y, targetYaw, RotationSpeed * Time.deltaTime);
			_transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
		}
	}
}
