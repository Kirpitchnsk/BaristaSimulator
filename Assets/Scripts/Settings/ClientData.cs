using System;
using UnityEngine;
using SibGameJam2026.Characters;
using SibGameJam2026.Items;

namespace SibGameJam2026.Settings {
	[Serializable]
	public class ClientData {
		[field: SerializeField] public ECharacterType ECharacterType { get; private set; }
		[field: SerializeField] public ItemId ItemId { get; private set; }
	}
}
