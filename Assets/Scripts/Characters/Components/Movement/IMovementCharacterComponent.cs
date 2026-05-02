using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public interface IMovementCharacterComponent : ICharacterComponent, IUpdatable {
		float MoveSpeed { get; }
		float RotationSpeed { get; }

		void SetMoveInput(Vector3 moveInput);
		void SetLookDirection(Vector3 lookDirection);
		void Stop();
	}
}
