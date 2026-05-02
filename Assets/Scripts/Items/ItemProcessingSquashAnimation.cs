using UnityEngine;

namespace SibGameJam2026 {
	/// <summary>
	/// Squash/stretch loop driven from the same MonoBehaviour Update as processing (надёжнее DOTween,
	/// если целевой Transform перезаписывается анимацией/префабом или твины не попадают в кадр).
	/// </summary>
	public struct ItemProcessingSquashState {
		public Transform Root;
		public Vector3 BaseScale;
		public Vector3 SquashScale;
		public float HalfCycleSeconds;
		public float Time;
		public bool Active;
	}

	public static class ItemProcessingSquashAnimation {
		public static void Start(
			ref ItemProcessingSquashState state,
			Transform root,
			float halfCycleDuration,
			float xzStretchMul,
			float ySquashMul) {
			if (root == null) {
				state.Active = false;
				return;
			}

			state.Root = root;
			state.BaseScale = root.localScale;
			var half = Mathf.Max(0.02f, halfCycleDuration);
			state.HalfCycleSeconds = half;
			state.SquashScale = new Vector3(
				state.BaseScale.x * xzStretchMul,
				state.BaseScale.y * ySquashMul,
				state.BaseScale.z * xzStretchMul);
			state.Time = 0f;
			state.Active = true;
			Tick(ref state, 0f);
		}

		public static void Tick(ref ItemProcessingSquashState state, float deltaTime) {
			if (!state.Active || state.Root == null)
				return;

			state.Time += deltaTime;
			var period = 2f * state.HalfCycleSeconds;
			var eased = (1f - Mathf.Cos(state.Time * 2f * Mathf.PI / period)) * 0.5f;
			state.Root.localScale = Vector3.LerpUnclamped(state.BaseScale, state.SquashScale, eased);
		}

		public static void Stop(ref ItemProcessingSquashState state) {
			if (state.Root != null && state.Active)
				state.Root.localScale = state.BaseScale;

			state.Active = false;
			state.Root = null;
		}
	}
}
