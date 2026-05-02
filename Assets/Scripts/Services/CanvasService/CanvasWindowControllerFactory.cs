using System;
using System.Collections.Generic;
using Zenject;


namespace Arenar.Services.UI
{
    public class CanvasWindowControllerFactory : IFactory<Type, CanvasWindowController>
    {
        private readonly DiContainer container;


        public CanvasWindowControllerFactory(DiContainer container) =>
            this.container = container;


        public CanvasWindowController Create(Type type)
        {
            CanvasWindowController windowController = (CanvasWindowController)container.Instantiate(type);

            container.Bind(type)
                .FromInstance(windowController)
                .AsSingle();

            return windowController;
        }

        public ITransitionWindowLayerController CreateTransitionWindowElementController(Type type)
        {
            ITransitionWindowLayerController transitionWindowLayerController =
                (ITransitionWindowLayerController)container.Instantiate(type);

            container.Bind(type)
                .FromInstance(transitionWindowLayerController)
                .AsSingle();

            return transitionWindowLayerController;
        }

        // TODO: check for need
        public void InjectIntoCollection(ICollection<CanvasWindowController> collection)
        {
            foreach (var element in collection)
            {
                container.Inject(element);
            }
        }
    }
}
