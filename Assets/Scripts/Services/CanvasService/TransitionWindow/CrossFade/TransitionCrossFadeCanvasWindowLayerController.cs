using System;
using DG.Tweening;


namespace Arenar.Services.UI
{
    public class TransitionCrossFadeCanvasWindowLayerController : ITransitionWindowLayerController
    {
        public ITransitionController TransitionController { private get; set; }
        public ICanvasService CanvasService { private get; set; }


        public void PlayDelayTransition<TFrom, TTo>(bool fromImmediately = true,
                                               bool toImmediately = true,
                                               float transitionToOffsetStart = 0f,
                                               Action callback = null)
            where TFrom : CanvasWindow
            where TTo : CanvasWindow
        {
            CanvasService.HideWindow<TFrom>(fromImmediately);
            DOVirtual.DelayedCall(transitionToOffsetStart, OnDelayedCall);


            void OnDelayedCall()
            {
                callback?.Invoke();
                CanvasService.ShowWindow<TTo>(toImmediately);
            }
        }

        public void PlayTransition<TFrom, TTo>(bool fromImmediately = true, bool toImmediately = true, Action callback = null)
            where TFrom : CanvasWindow
            where TTo : CanvasWindow
        {
            callback?.Invoke();

            CanvasService.HideWindow<TFrom>(fromImmediately, 
                () => CanvasService.ShowWindow<TTo>(toImmediately));
        }
    }
}
