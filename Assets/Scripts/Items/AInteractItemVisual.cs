using UnityEngine;

namespace SibGameJam2026{
    public abstract class AInteractItemVisual : MonoBehaviour, IInteractable
    {
        public abstract void OnInteract(InteractContext context);
    }

    public interface IInteractable
    {
        void OnInteract(InteractContext context);
    }
}