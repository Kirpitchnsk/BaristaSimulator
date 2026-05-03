using UnityEngine;

namespace SibGameJam2026{
    public abstract class AInteractItemVisual : MonoBehaviour, IInteractable {
        public abstract string InteractItemName { get; }
        public abstract void OnInteract(InteractContext context);
    }

    public interface IInteractable {
        string InteractItemName { get; }
        
        void OnInteract(InteractContext context);
    }
}