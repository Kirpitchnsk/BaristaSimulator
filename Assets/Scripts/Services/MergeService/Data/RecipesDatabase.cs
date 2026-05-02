using System.Collections.Generic;
using SibGameJam2026.Items;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.MergeService {
	[CreateAssetMenu(fileName = "RecipesDatabase", menuName = "Data/RecipesDatabase", order = 53)]
	public class RecipesDatabase : ScriptableObjectInstaller<RecipesDatabase> {
		[SerializeField] private List<RecipeDefinition> _recipes = new();

		public IReadOnlyList<RecipeDefinition> Recipes => _recipes;

		public override void InstallBindings() {
			Container.Bind<RecipesDatabase>().FromInstance(this).AsSingle();
		}

		public bool TryGetRecipeByOutputItemId(ItemId outputItemId, out RecipeDefinition recipe) {
			recipe = _recipes.Find(x => x != null && x.OutputItemId == outputItemId);
			return recipe != null;
		}
	}
}
