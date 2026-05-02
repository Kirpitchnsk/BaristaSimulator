using System;
using UnityEngine;


namespace Arenar.Services.UI
{
    [Serializable]
    public sealed class CanvasWindowsSettings
    {
        [SerializeField] private string sceneRootName = default;
        [SerializeField] private CanvasWindow[] canvasWindows = default;
        [SerializeField] private CanvasWindow defaultWindow = default;
		[SerializeField] private TransitionCanvasWindow transitionWindow = default;
		[SerializeField] private Vector2Int canvasScalerBaseResolution = new(1080, 1920);
		[SerializeField] private RenderMode renderMode = default;
        [SerializeField] private int sortingOffset = 100;
        [SerializeField] private float planeDistance = 0.3f;


        public string SceneRootName => sceneRootName;
        public CanvasWindow[] CanvasWindows => canvasWindows;
        public CanvasWindow DefaultWindow => defaultWindow;
		public TransitionCanvasWindow TransitionWindow => transitionWindow;
		public Vector2Int CanvasScalerBaseResolution => canvasScalerBaseResolution;
		public RenderMode RenderMode => renderMode;
        public int SortingOffset => sortingOffset;
        public float PlaneDistance => planeDistance;
    }
}
