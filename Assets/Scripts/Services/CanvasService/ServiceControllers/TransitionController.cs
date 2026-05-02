using System;
using System.Collections.Generic;
using UnityEngine;


namespace Arenar.Services.UI
{
    public class TransitionController : ITransitionController
	{
        private List<ITransitionWindowLayerController> controllers = new();


        public ITransitionWindow TransitionWindow { get; private set; }


        public void SetupTransitionWindow(ITransitionWindow transitionWindow) =>
            TransitionWindow = transitionWindow;
        
		public void PlayTransition<TElement, TFrom, TTo>(bool fromImmediately = true, bool toImmediately = true, Action callback = null)
            where TElement : ITransitionWindowLayerController
            where TFrom : CanvasWindow
			where TTo : CanvasWindow
		{
            foreach (var controller in controllers)
            {
                if (controller is not TElement transitionWindowElementController)
                    continue;
                
                transitionWindowElementController.PlayTransition<TFrom, TTo>(fromImmediately, toImmediately, callback);
                return;
            }
		}

        public bool TryGetTransition<TElement>(out TElement tElement)
            where TElement : ITransitionWindowLayerController
        {
            foreach (var controller in controllers)
            {
                if (controller is not TElement transitionWindowElementController)
                    continue;

                tElement = transitionWindowElementController;
                return true;
            }
            
            Debug.LogError($"Not found {nameof(TElement)} transition class. Return another class.");
            tElement = (TElement)controllers[0];
            return false;
        }

        public void SetupTransitionWindowController(ITransitionWindowLayerController transitionWindowLayer) =>
            controllers.Add(transitionWindowLayer);
    }
}
