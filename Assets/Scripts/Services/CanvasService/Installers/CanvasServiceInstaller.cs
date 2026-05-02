using Arenar.Services.UI;
using Zenject;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Arenar.Installers
{
    public sealed class CanvasServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
			Container.BindInterfacesAndSelfTo<CanvasService>()
					 .AsSingle()
					 .NonLazy();

			BindFactories();
			BindServiceControllers();
		}


		private void BindFactories()
		{
			Container.BindFactory<Object, Transform, CanvasWindow, CanvasWindow.Factory>()
					 .FromFactory<CanvasWindowFactory>();

			Container.Bind<CanvasWindowControllerFactory>()
					 .FromNew()
					 .AsSingle();
		}

		private void BindServiceControllers()
		{
			Container.BindInterfacesAndSelfTo<TransitionController>()
					 .AsSingle()
					 .NonLazy();
		}
	}
}
