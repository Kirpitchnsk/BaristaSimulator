using System;
using UnityEngine;

namespace SibGameJam2026.Characters {
	public sealed class NpcControlStateAuthoring {
		public Transform OrderPoint { get; }
		public Transform ExitPoint { get; }
		public float ArriveDistance { get; }
		public float MoveSpeed { get; }
		public GameObject NormalVisualRoot { get; }
		public GameObject TransformedVisualRoot { get; }
		public bool DestroyOnExit { get; }

		public Action<EClientState> StateChanged;
		public Action ReachedOrderPoint;
		public Action CookTimedOut;
		public Action Transformed;
		public Action Left;

		public NpcControlStateAuthoring(
			CarManager carManager,
			float arriveDistance = 0.6f,
			float moveSpeed = 3.5f,
			GameObject normalVisualRoot = null,
			GameObject transformedVisualRoot = null,
			bool destroyOnExit = true
		) {
			if (carManager == null)
				throw new ArgumentNullException(nameof(carManager));

			OrderPoint = carManager.OrderPoint;
			ExitPoint = carManager.ReturnToPoolPoint;
			ArriveDistance = arriveDistance;
			MoveSpeed = moveSpeed;
			NormalVisualRoot = normalVisualRoot;
			TransformedVisualRoot = transformedVisualRoot;
			DestroyOnExit = destroyOnExit;
		}

		public void RaiseStateChanged(EClientState state) => StateChanged?.Invoke(state);
		public void RaiseReachedOrderPoint() => ReachedOrderPoint?.Invoke();
		public void RaiseCookTimedOut() => CookTimedOut?.Invoke();
		public void RaiseTransformed() => Transformed?.Invoke();
		public void RaiseLeft() => Left?.Invoke();
	}
}
