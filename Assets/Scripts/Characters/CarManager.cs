using UnityEngine;

namespace SibGameJam2026.Characters {
	/// <summary>
	/// Точки сценария клиента: спавн, заказ, возврат в пул.
	/// </summary>
	public class CarManager : MonoBehaviour {
		[field: SerializeField] public Transform ClientSpawnPoint { get; private set; }
		[field: SerializeField] public Transform OrderPoint { get; private set; }
		[field: SerializeField] public Transform ReturnToPoolPoint { get; private set; }
	}
}
