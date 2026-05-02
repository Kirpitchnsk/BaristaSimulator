using System.Collections.Generic;
using SibGameJam2026.Items;
using UnityEngine;

namespace SibGameJam2026.MergeService {
	[CreateAssetMenu(fileName = "RecipeDefinition", menuName = "Data/RecipeDefinition", order = 52)]
	public class RecipeDefinition : ScriptableObject {
		[SerializeField] private string _recipeId;
		[SerializeField] private List<ItemId> _inputItemIds = new();
		[SerializeField] private ItemId _outputItemId;

		public string RecipeId => _recipeId;
		public IReadOnlyList<ItemId> InputItemIds => _inputItemIds;
		public ItemId OutputItemId => _outputItemId;
	}
}
