using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


namespace Arenar.AudioSystem
{
    [CreateAssetMenu(menuName = "Audio System/Sounds Data")]
    public class SoundsLibrary : ScriptableObjectInstaller
    {
        [Header("Kitty sounds")]
        [SerializeField] private SerializableDictionary<GroundType, AudioClip[]> groundStepSounds = default;
        [SerializeField] private AudioClip[] attackSounds = default;
        
        
        public AudioClip GetRandomAttackSound() =>
            attackSounds[Random.Range(0, attackSounds.Length)];
        
        public AudioClip GetRandomGroundStepSound(GroundType type)
        {
            AudioClip[] clips = groundStepSounds[type];
            return clips[Random.Range(0, clips.Length)];
        }
    }
}
