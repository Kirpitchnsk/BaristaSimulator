using System;
using DG.Tweening;
using UnityEngine;


namespace Arenar.Services.UI
{
    public abstract class CanvasWindowLayer : MonoBehaviour
    {
	    private Canvas canvas = default;
	    
	    
		public Action<CanvasWindowLayer> onCanvasLayerShowBegin;
		public Action<CanvasWindowLayer> onCanvasLayerShowEnd;
		public Action<CanvasWindowLayer> onCanvasLayerHideBegin;
		public Action<CanvasWindowLayer> onCanvasLayerHideEnd;


		protected Canvas Canvas
        {
            get
            {
                if (canvas != null)
                    return canvas;

                if (gameObject.TryGetComponent(out Canvas objectCanvas))
                {
                    canvas = objectCanvas;
                    return canvas;
                }

                canvas = gameObject.AddComponent<Canvas>();
                return canvas;
            }
        }


		public virtual void Initialize() {}

		public virtual void DeInitialize() {}

		public virtual void ShowWindowLayerStart()
		{
			Canvas.enabled = true;
			onCanvasLayerShowBegin?.Invoke(this);
		}

		public virtual void ShowWindowLayerComplete() =>
			onCanvasLayerShowEnd?.Invoke(this);

		public virtual void HideWindowLayerStart() =>
			onCanvasLayerHideBegin?.Invoke(this);

		public virtual void HideWindowLayerComplete()
		{
			onCanvasLayerHideEnd?.Invoke(this);
			Canvas.enabled = false;
		}
    }
}
