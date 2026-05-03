using System;
using UnityEngine;

namespace Arenar.Options
{
    [Serializable]
    public class MusicOption : IOption
    {
        private const float MAX_VOLUME = 1f;
        
        
        private bool isActive;
        private float volume;


        public MusicOption()
        {
            isActive = true;
            volume = MAX_VOLUME;
        }
        
        
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        
        public float Volume
        {
            get => volume;
            set => volume = Mathf.Clamp01(value);
        }
    }
}
