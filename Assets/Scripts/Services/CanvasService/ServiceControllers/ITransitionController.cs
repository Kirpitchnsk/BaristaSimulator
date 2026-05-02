using System;

namespace Arenar.Services.UI
{
    public interface ITransitionController
	{
        ITransitionWindow TransitionWindow { get; }
        

        void SetupTransitionWindow(ITransitionWindow transitionWindow);

        void SetupTransitionWindowController(ITransitionWindowLayerController transitionWindowLayer);

		void PlayTransition<TElement, TFrom, TTo>(bool fromImmediately = true, bool toImmediately = true, Action callback = null)
            where TElement : ITransitionWindowLayerController
            where TFrom : CanvasWindow
			where TTo : CanvasWindow;

        bool TryGetTransition<TElement>(out TElement tElement)
            where TElement : ITransitionWindowLayerController;
    }
}
