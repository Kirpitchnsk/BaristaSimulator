using System;
using UnityEngine;

namespace Arenar.Services.UI
{
    [Serializable]
    public sealed class ReviveCanvasParameters
    {
        [SerializeField] private int noThanksAppearButtonTime = 3;
        [SerializeField] private float openWindowDelay = 1.5f;


        public int NoThanksAppearButtonTime => noThanksAppearButtonTime;
        public float OpenWindowDelay => openWindowDelay;
    }
}
