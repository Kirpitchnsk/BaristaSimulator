using UnityEngine;
using Zenject;
using Arenar.Services.UI;


namespace Arenar.Installers
{
	[CreateAssetMenu(fileName = "CanvasWindowsSettingsInstaller", menuName = "Services/CanvasService/CanvasWindowsSettingsInstaller")]
	public class CanvasServiceScriptableInstaller : ScriptableObjectInstaller
	{
		[SerializeField] private CanvasWindowsSettings canvasWindowsSettings = default;


		public override void InstallBindings()
		{
			Container.Bind<CanvasWindowsSettings>()
					 .FromInstance(canvasWindowsSettings)
					 .AsSingle();
		}
	}
}
