using UnityEngine;
using Zenject;


namespace Arenar.Services.UI
{
    public class CanvasWindowFactory : IFactory<Object, Transform, CanvasWindow>
    {
        readonly DiContainer container;


        public CanvasWindowFactory(DiContainer container)
        {
            this.container = container;
        }

        public CanvasWindow Create(Object prefab, Transform parent)
        {
            return container.InstantiatePrefab(prefab, parent)
                            .GetComponent<CanvasWindow>();
        }
    }
}
