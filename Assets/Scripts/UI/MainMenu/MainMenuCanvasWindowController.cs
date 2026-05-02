using SibGameJam2026.Services;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arenar.Services.UI
{
	public class MainMenuCanvasWindowController : CanvasWindowController
	{
		private MainMenuCanvasWindow _mainMenuCanvasWindow;
		private MainMenuButtonsCanvasWindowLayer _buttons;
		private IGameService _gameService;
		
		public MainMenuCanvasWindowController(IInputService inputService, IGameService gameService) : base(inputService) {
			_gameService = gameService;
		}

		public override void Initialize(ICanvasService canvasService)
		{
			base.Initialize(canvasService);
			
			_mainMenuCanvasWindow = canvasService
				.GetWindow<MainMenuCanvasWindow>();

			_buttons = _mainMenuCanvasWindow.GetWindowLayer<MainMenuButtonsCanvasWindowLayer>();

			_buttons.StartButton.onClick.AddListener(StartButton_OnClick);
			_buttons.AuthorsButton.onClick.AddListener(Authors_OnClick);
			_buttons.ExitButton.onClick.AddListener(Exit_OnClick);
		}

		private void StartButton_OnClick()
		{
			_gameService.StartGame();
			canvasService.ShowWindow<GameplayCanvasWindow>();
		}

		private void Authors_OnClick()
		{
			
		}

		private void Exit_OnClick()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		protected override void OnWindowShowEnd_SelectElements() {
			SetMainMenuButtonsInteractable(true);
		}

		protected override void OnWindowHideBegin_DeselectElements() {
			SetMainMenuButtonsInteractable(false);
		}

		private void SetMainMenuButtonsInteractable(bool interactable) {
			if (_buttons == null)
				return;

			_buttons.StartButton.interactable = interactable;
			_buttons.AuthorsButton.interactable = interactable;
			_buttons.ExitButton.interactable = interactable;
		}
	}
}