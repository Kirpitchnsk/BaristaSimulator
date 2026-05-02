using System;
using UnityEngine;


namespace Arenar.Services.UI
{
	public interface ICanvasService
	{
		public event Action<CanvasWindow> OnShowWindow;
		public event Action<CanvasWindow> OnShowPopup;
		public event Action<CanvasWindow> OnHidePopup;


		public Canvas RootCanvas { get; }
		public ITransitionController TransitionController { get; }

        public CanvasWindow ActiveWindow { get; }

        public bool IsCanvasInitialized { get; }


        void ShowWindow(CanvasWindow window, bool immediately = false, Action action = null);
		T ShowWindow<T>(bool immediately = false, Action action = null) where T : CanvasWindow;

		void HideWindow(CanvasWindow window, bool immediately = false, Action action = null);
		T HideWindow<T>(bool immediately = false, Action action = null) where T : CanvasWindow;

		void ShowPopup(CanvasWindow window, bool immediately = false);
		void HidePopup();

		T GetWindow<T>() where T : CanvasWindow;
	}
}
