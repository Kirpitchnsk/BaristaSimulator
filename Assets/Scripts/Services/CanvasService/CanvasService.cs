using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;


namespace Arenar.Services.UI
{
    public class CanvasService : ICanvasService, IInitializable
    {
        public event Action<CanvasWindow> OnShowWindow;
		public event Action<CanvasWindow> OnShowPopup;
		public event Action<CanvasWindow> OnHidePopup;


		private Stack<CanvasWindow> windowsStack;
        private Dictionary<Type, CanvasWindow> allWindowsPool;
		private CanvasWindowsSettings canvasWindowsSettings;
		private CanvasWindow.Factory canvasWindowFactory;
		private CanvasWindowControllerFactory canvasControllerFactory;
		private CanvasWindow defaultWindow;


		public CanvasWindow ActiveWindow => windowsStack.Peek();
        public bool IsCanvasInitialized { get; private set; } = false;
        public Canvas RootCanvas { get; private set; }
        public ITransitionController TransitionController { get; private set; }


        public CanvasService(CanvasWindowsSettings canvasWindowsSettings,
							 CanvasWindow.Factory canvasWindowFactory,
                             CanvasWindowControllerFactory canvasControllerFactory)
		{
			this.canvasWindowsSettings = canvasWindowsSettings;
			this.canvasWindowFactory = canvasWindowFactory;
			this.canvasControllerFactory = canvasControllerFactory;
        }


		[Inject]
		public void Construct(ITransitionController transitionController) =>
			TransitionController = transitionController;

		public void Initialize()
        {
            allWindowsPool = new Dictionary<Type, CanvasWindow>();
            windowsStack = new Stack<CanvasWindow>();

            RootCanvas = CreateRootCanvas();
            CheckEventSystem();
            GenerateCanvasWindows(RootCanvas);
            GenerateElementCanvasControllers();
            SetupDefaultWindow();

            Application.targetFrameRate = 60;

            IsCanvasInitialized = true;
        }

		public virtual void ShowWindow(CanvasWindow window, bool immediately = false, Action onComplete = null)
		{
			windowsStack.Clear();
			windowsStack.Push(window);

			window.Canvas.sortingOrder = window.OverrideSorting
				? window.Canvas.sortingOrder
				: windowsStack.Count * canvasWindowsSettings.SortingOffset;

			window.Show(immediately, onComplete);
			OnShowWindow?.Invoke(window);
		}

		public virtual T ShowWindow<T>(bool immediately = false, Action onComplete = null) where T : CanvasWindow
		{
			T window = (T)allWindowsPool[typeof(T)];
			ShowWindow(window, immediately, onComplete);
			return window;
		}

		public virtual void HideWindow(CanvasWindow window, bool immediately = false, Action onComplete = null)
		{
			window.Hide(immediately, onComplete);
		}

		public virtual T HideWindow<T>(bool immediately = false, Action onComplete = null) where T : CanvasWindow
		{
			T window = (T)allWindowsPool[typeof(T)];
			HideWindow(window, immediately, onComplete);
			return window;
		}

		public virtual void ShowPopup(CanvasWindow window, bool immediately = false)
		{
			windowsStack.Push(window);
			window.Canvas.sortingOrder = windowsStack.Count * canvasWindowsSettings.SortingOffset;
			window.Show();
			OnShowPopup?.Invoke(window);
		}

		public virtual void HidePopup()
		{
			if (windowsStack.Count <= 1)
			{
				Debug.LogError($"Can't hide the last {nameof(CanvasWindow)}!");
				return;
			}

			var window = windowsStack.Pop();
			window.Canvas.sortingOrder = 0;
			window.Hide();
			OnHidePopup?.Invoke(window);
		}

		public virtual T GetWindow<T>() where T : CanvasWindow
		{
			T window = (T)allWindowsPool[typeof(T)];
			return window;
		}

		private Canvas CreateRootCanvas()
		{
			GameObject rootObject = GameObject.Find(canvasWindowsSettings.SceneRootName);

			if (rootObject == null)
				rootObject = new GameObject(canvasWindowsSettings.SceneRootName);

			Canvas canvas = new GameObject($"{nameof(Canvas)}", typeof(Canvas), typeof(CanvasScaler)).GetComponent<Canvas>();
			canvas.transform.SetParent(rootObject.transform);
			canvas.renderMode = canvasWindowsSettings.RenderMode;
			canvas.planeDistance = canvasWindowsSettings.PlaneDistance;
			canvas.worldCamera = Camera.main;

			var canvasScaler = canvas.GetComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = canvasWindowsSettings.CanvasScalerBaseResolution;
			canvasScaler.matchWidthOrHeight = 0.5f;

			return canvas;
		}

		private void CheckEventSystem()
		{
			if (Object.FindObjectOfType<EventSystem>() == null)
			{
				var eventSystem = new GameObject($"{nameof(EventSystem)}", typeof(EventSystem), typeof(StandaloneInputModule));
			}
		}

        private void GenerateElementCanvasControllers()
        {
            Type baseType = typeof(CanvasWindowController);
            Type[] implementations = GetImplementations(baseType);
            List<CanvasWindowController> canvasWindowElementControllers = new(implementations.Length);

            foreach (var implementation in implementations)
            {
	            CanvasWindowController controller = canvasControllerFactory.Create(implementation);
	            controller.Initialize(this);
	            canvasWindowElementControllers.Add(controller);
            }
        }

		private void GenerateCanvasWindows(Canvas rootCanvas)
		{
			foreach (var canvasWindowPrefab in canvasWindowsSettings.CanvasWindows)
			{
				CanvasWindow canvasWindow = canvasWindowFactory.Create(canvasWindowPrefab.gameObject, rootCanvas.transform);
                allWindowsPool.Add(canvasWindow.GetType(), canvasWindow);

                if (canvasWindowPrefab == canvasWindowsSettings.DefaultWindow)
                    defaultWindow = canvasWindow;
            }

            foreach (var window in allWindowsPool)
            {
                window.Value.Initialize();
                window.Value.Hide(true);
            }

            TrySetupTransitionWindow();
		}

		private void SetupDefaultWindow()
		{
			if (defaultWindow == null)
			{
				Debug.LogError($"Canvas Default window must be not null! Assign it in <b>{nameof(CanvasWindowsSettings)}</b>!");
				return;
			}

			foreach (var window in allWindowsPool.Values)
				window.gameObject.SetActive(false);

			ShowWindow(defaultWindow, true);
		}

        private void TrySetupTransitionWindow()
		{
            Type baseType = typeof(ITransitionWindowLayerController);
            Type[] implementations = GetImplementations(baseType);

            foreach (var implementation in implementations)
            {
                var transitionWindowElementController =
                    canvasControllerFactory.CreateTransitionWindowElementController(implementation);

                transitionWindowElementController.CanvasService = this;
                transitionWindowElementController.TransitionController = TransitionController;
                TransitionController.SetupTransitionWindowController(transitionWindowElementController);
            }

            if (canvasWindowsSettings.TransitionWindow == null)
	            return;
            
            CanvasWindow transitionWindow = canvasWindowFactory
                .Create(canvasWindowsSettings.TransitionWindow, RootCanvas.transform);

            transitionWindow.Initialize();
            TransitionController.SetupTransitionWindow((ITransitionWindow)transitionWindow);
        }


		/// <summary>Returns non-abstract and non-generic implementations of type.</summary>
		public static Type[] GetImplementations(Type baseType)
		{
			bool ImplementationCondition(Type type) =>
				baseType.IsAssignableFrom(type)
				&& type != baseType
				&& !type.IsAbstract
				&& !type.IsGenericType;

			return GetAllMatchingTypes(baseType, ImplementationCondition);
		}

		private static Type[] GetAllMatchingTypes(Type baseType, Func<Type, bool> predicate)
		{
			return AppDomain.CurrentDomain
							.GetAssemblies()
							.SelectMany(s => s.GetTypes())
							.Where(predicate)
							.ToArray();
		}
	}
}
