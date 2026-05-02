using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SibGameJam2026.Cameras {
	[Serializable]
	public class CameraEntry {
		[field: SerializeField] public ECameraType ECameraType { get; private set; }
		[field: SerializeField] public AssetReference CameraPrefab { get; private set; }
	}
}
