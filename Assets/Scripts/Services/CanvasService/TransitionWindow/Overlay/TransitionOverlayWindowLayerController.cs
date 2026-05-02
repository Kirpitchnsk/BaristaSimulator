using System;


namespace Arenar.Services.UI
{
    public class TransitionOverlayCanvasWindowController : ITransitionWindowLayerController
    {
        private TransitionOverlayCanvasWindowLayer transitionOverlayCanvasWindowLayer;


        private TransitionOverlayCanvasWindowLayer TransitionOverlayCanvasWindowLayer
        {
            get
            {
                if (transitionOverlayCanvasWindowLayer == null)
                {
                    CanvasWindow transitionCanvasWindow = (CanvasWindow) TransitionController.TransitionWindow;
                    transitionOverlayCanvasWindowLayer = transitionCanvasWindow
                        .GetWindowLayer<TransitionOverlayCanvasWindowLayer>();
                }

                return transitionOverlayCanvasWindowLayer;
            }
        }


        public ITransitionController TransitionController { private get; set; }
        public ICanvasService CanvasService { private get; set; }


        public void PlayTransition<TFrom, TTo>(bool fromImmediately = true, bool toImmediately = true, Action callback = null)
            where TFrom : CanvasWindow
            where TTo : CanvasWindow
        {
            TransitionOverlayCanvasWindowLayer.SetupCallback(ShowCanvasWindow);
            TransitionOverlayCanvasWindowLayer.StartOverlayAnimation();


            void ShowCanvasWindow()
            {
                 callback?.Invoke();
                CanvasService.HideWindow<TFrom>(fromImmediately);
                CanvasService.ShowWindow<TTo>(toImmediately);
            }
        }
    }
}
