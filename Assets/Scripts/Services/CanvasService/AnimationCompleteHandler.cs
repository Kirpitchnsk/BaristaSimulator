using Arenar.Services.UI;
using UnityEngine;


namespace Arenar.Helpers
{
    public class AnimationCompleteHandler : StateMachineBehaviour
    {
        [SerializeField] private AnimationType animationType = default;


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.gameObject.TryGetComponent(out CanvasWindowLayer canvasWindowLayer))
            {
                switch (animationType)
                {
                    case AnimationType.Show:
                        canvasWindowLayer.ShowWindowLayerComplete();
                        break;

                    case AnimationType.Hide:
                        canvasWindowLayer.HideWindowLayerStart();
                        break;

                    default:
                        Debug.LogError($"Animation Type <b>{animationType}</b> is not supported!");
                        return;
                }
            }

            base.OnStateExit(animator, stateInfo, layerIndex);
        }


        private enum AnimationType : byte
        {
            None = 0,
            Show = 1,
            Hide = 2,
        }
    }
}
