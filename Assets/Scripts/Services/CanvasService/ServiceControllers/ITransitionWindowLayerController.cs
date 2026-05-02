using System;


namespace Arenar.Services.UI
{
    public interface ITransitionWindowLayerController
    {
        ITransitionController TransitionController { set; }

        ICanvasService CanvasService { set; }


        void PlayTransition<TFrom, TTo>(bool fromImmediately = true, bool toImmediately = true, Action callback = null)
            where TFrom : CanvasWindow
            where TTo : CanvasWindow;
    }
}
