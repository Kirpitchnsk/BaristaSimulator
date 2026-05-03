using System;
using SibGameJam2026.Characters;
using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public class NpcControlStateCharacterComponent : INpcControlStateCharacterComponent, IUpdatable {
		private readonly ACharacter _character;
		private readonly NpcControlStateAuthoring _authoring;
		private EClientState _state = EClientState.WalkToOrder;

		public ACharacter Character => _character;

		public EClientState State => _state;

		public event Action<EClientState> StateChanged;

		public NpcControlStateCharacterComponent(
			ACharacter character,
			NpcControlStateAuthoring authoring
		) {
			_character = character;
			_authoring = authoring;
		}

		public void SetState(EClientState next) {
			if (_state == EClientState.Finished)
				return;
			if (_state == next)
				return;

			_state = next;
			OnEnteredState(next);
			StateChanged?.Invoke(next);
			_authoring.RaiseStateChanged(next);
		}

		public void OnUpdate() {
			const int maxStepsPerFrame = 8;
			for (var step = 0; step < maxStepsPerFrame && _state != EClientState.Finished; step++) {
				var before = _state;
				switch (_state) {
					case EClientState.WalkToOrder:
						UpdateWalkToOrder();
						break;
					case EClientState.WaitInteraction:
						break;
					case EClientState.WaitCooking:
						UpdateWaitCooking();
						break;
					case EClientState.TransformCreatureSuccess:
					case EClientState.TransformCreatureFailed:
						UpdateTransform();
						break;
					case EClientState.Leave:
						UpdateLeave();
						break;
				}

				if (_state == before)
					break;
			}
		}

		private void OnEnteredState(EClientState entered) {
			switch (entered) {
				case EClientState.WaitCooking:
					StopMovement();
					break;
				case EClientState.WaitInteraction:
					StopMovement();
					break;
			}
		}

		private void UpdateWalkToOrder() {
			var order = _authoring.OrderPoint;
			if (order == null) {
				SetState(EClientState.WaitInteraction);
				_authoring.RaiseReachedOrderPoint();
				return;
			}

			if (MoveTowards(order.position)) {
				_authoring.RaiseReachedOrderPoint();
				SetState(EClientState.WaitInteraction);
			}
		}

		private void UpdateWaitCooking() {
			// Таймер ожидания теперь считается в LevelService.
		}

		private void UpdateTransform() {
			if (_authoring.NormalVisualRoot != null)
				_authoring.NormalVisualRoot.SetActive(false);
			if (_authoring.TransformedVisualRoot != null)
				_authoring.TransformedVisualRoot.SetActive(true);

			_authoring.RaiseTransformed();
			SetState(EClientState.Leave);
		}

		private void UpdateLeave() {
			var exit = _authoring.ExitPoint;
			if (exit == null) {
				FinishExit();
				return;
			}

			if (MoveTowards(exit.position))
				FinishExit();
		}

		private void FinishExit() {
			StopMovement();
			_authoring.RaiseLeft();
			if (_state != EClientState.Finished) {
				_state = EClientState.Finished;
				StateChanged?.Invoke(EClientState.Finished);
				_authoring.RaiseStateChanged(EClientState.Finished);
			}

			if (_authoring.DestroyOnExit)
				UnityEngine.Object.Destroy(_character.gameObject);
		}

		private bool MoveTowards(Vector3 worldTarget) {
			var position = _character.transform.position;
			var delta = worldTarget - position;
			delta.y = 0f;
			var distance = delta.magnitude;
			if (distance <= _authoring.ArriveDistance) {
				StopMovement();
				return true;
			}

			var direction = delta / distance;

			if (_character.TryGetComponent<IMovementCharacterComponent>(out var movement)) {
				movement.SetMoveInput(direction);
				movement.SetLookDirection(direction);
			} else {
				var cc = _character.Data != null ? _character.Data.CharacterController : null;
				var speed = _authoring.MoveSpeed;
				if (cc != null)
					cc.Move(direction * (speed * Time.deltaTime));
				else
					_character.transform.position += direction * (speed * Time.deltaTime);

				if (direction.sqrMagnitude > 0f) {
					var look = Quaternion.LookRotation(direction, Vector3.up);
					_character.transform.rotation = Quaternion.RotateTowards(
						_character.transform.rotation,
						look,
						540f * Time.deltaTime);
				}
			}

			return false;
		}

		private void StopMovement() {
			if (_character.TryGetComponent<IMovementCharacterComponent>(out var movement))
				movement.Stop();
		}
	}
}
