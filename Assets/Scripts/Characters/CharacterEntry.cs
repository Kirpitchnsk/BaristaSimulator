using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SibGameJam2026.Characters {
	[Serializable]
	public class CharacterEntry {
		[field: SerializeField] public ECharacterType ECharacterType { get; private set; }
		[field: SerializeField] public int Health { get; private set; } = 100;
		[field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
		[field: SerializeField] public float RotationSpeed { get; private set; } = 360f;
		[field: SerializeField] public float JumpForce { get; private set; } = 5f;
		[field: SerializeField] public AssetReference CharacterPrefab { get; private set; }
	}
}
