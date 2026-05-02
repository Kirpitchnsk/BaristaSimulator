using System;
using SibGameJam2026.Services;
using Zenject;


namespace Arenar.Services.UI
{
	public abstract class CanvasWindowController
	{
		protected ICanvasService canvasService;
		protected IInputService inputService;
		
		
		public CanvasWindowController(IInputService inputService) =>
			this.inputService = inputService;
		

		public virtual void Initialize(ICanvasService canvasService) =>
			this.canvasService = canvasService;

		protected abstract void OnWindowShowEnd_SelectElements();
		protected abstract void OnWindowHideBegin_DeselectElements();
		
		
		public class Factory : PlaceholderFactory<Type, CanvasWindowController> {}
	}
}
