using System;
using SibGameJam2026.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SibGameJam2026.MergeService {
	[Serializable]
	public struct Item {
		public ItemId ItemId;
		public string Name;
		public string Description;
		public EItemType ItemType;
		public Sprite Icon;
		public AssetReference Prefab;
	}

	public enum EItemType : byte {
		None = 0,
		CoffeeBean = 1,
		Liquid = 2,
		Food = 3,
		Ingredient = 4,
	}
}