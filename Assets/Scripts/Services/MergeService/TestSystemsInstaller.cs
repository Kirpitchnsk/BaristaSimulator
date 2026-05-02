using System.Collections.Generic;
using System.Linq;
using SibGameJam2026.Items;
using SibGameJam2026.Testing;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.MergeService {
	public class TestSystemsInstaller : MonoInstaller {
		[SerializeField] private List<string> _repiceTestInputItemIds = new();
		[SerializeField] private bool _enableUiInputMonitor = true;

		public override void InstallBindings() {
			if (_enableUiInputMonitor)
				Container.BindInterfacesAndSelfTo<UiInputMonitor>().AsSingle().NonLazy();

			#if UNITY_EDITOR
			TestRecipe();
			#endif
		}

		private void TestRecipe()
		{
			var inputIds = _repiceTestInputItemIds
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.Select(s => new ItemId(s))
				.ToArray();

			Container.Bind<ItemId[]>()
				.WithId("RepiceTestInputIds")
				.FromInstance(inputIds)
				.AsSingle();

			Container.BindInterfacesAndSelfTo<RecipeTest>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<RepiceTest>().AsSingle().NonLazy();
		}
	}
}
