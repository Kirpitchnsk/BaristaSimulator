using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


namespace Arenar.AudioSystem
{
    [CreateAssetMenu(menuName = "Audio System/Sounds Data")]
    public class SoundsLibrary : ScriptableObjectInstaller
    {
        [SerializeField] private SerializableDictionary<ESoundType, AudioClip[]> _sounds = default;
        [SerializeField] private AudioClip[] attackSounds = default;


        public AudioClip GetRandomAttackSound() =>
            attackSounds[Random.Range(0, attackSounds.Length)];
        
        public AudioClip GetRandomGroundStepSound(ESoundType type)
        {
            AudioClip[] clips = _sounds[type];
            return clips[Random.Range(0, clips.Length)];
        }

        public AudioClip GetInteractionClip(ESoundType type)
        {
            if (type == ESoundType.None || _sounds == null)
                return null;

            foreach (var pair in _sounds)
            {
                if (pair.Key != type)
                    continue;

                return pair.Value[Random.Range(0, pair.Value.Length)];
            }

            Debug.LogWarning($"[{nameof(SoundsLibrary)}] No interaction clip for {type}.");
            return null;
        }
    }
}
