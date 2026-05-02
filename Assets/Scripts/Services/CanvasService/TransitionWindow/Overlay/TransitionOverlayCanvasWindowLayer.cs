using System;
using UnityEngine;


namespace Arenar.Services.UI
{
    public class TransitionOverlayCanvasWindowLayer : CanvasWindowLayer, ITransitionWindowElement
    {
        [SerializeField] private Animator overlayAnimator;
        [SerializeField] private string overlayAnimationName;
        
        private Action completeCloseAction;


        public void StartOverlayAnimation()
        {
            Canvas.enabled = true;
            overlayAnimator.Play(overlayAnimationName);
        }
        
        public void SetupCallback(Action action)
        {
            completeCloseAction = action;
        }

        // for animation
        public void OnCompleteHideAnimation()
        {
            completeCloseAction?.Invoke();
            completeCloseAction = null;
        }
        
        // for animation
        public void OnCompleteShowAnimation()
        {
            Canvas.enabled = false;
        }
    }
}
