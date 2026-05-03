using System;
using UnityEngine;


namespace Arenar.AudioSystem
{
    [Serializable]
    public class AmbientLibrary
    {
        [SerializeField] private SerializableDictionary<AmbientType, AudioClip> ambients = default;


        public AudioClip GetAmbientByType(AmbientType type)
        {
            foreach (var ambient in ambients)
            {
                if (ambient.Key != type)
                    continue;

                return ambient.Value;
            }

            Debug.LogError($"Not found ambient {type}!");
            return null;
        }
    }
}
