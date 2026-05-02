using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SibGameJam2026.Settings {
	[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Data/GameSettingsData")]
	public class GameSettingsData : ScriptableObject {
		[SerializeField] private string _key;
		[SerializeField] private List<ClientData> _clientData = new();
		[SerializeField] private AssetReference _location;

		public string Key => _key;
		public IReadOnlyList<ClientData> ClientData => _clientData;
		public AssetReference Location => _location;
	}
}
