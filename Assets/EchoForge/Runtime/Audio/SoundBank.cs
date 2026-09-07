using UnityEngine;

namespace EchoForge.Audio
{
    /// <summary>Project-generated original audio, one clip per Sfx cue plus ambience beds.
    /// See Assets/Documentation/AUDIO_PROVENANCE.md.</summary>
    [CreateAssetMenu(menuName = "Echo Forge/Sound Bank", fileName = "SoundBank")]
    public sealed class SoundBank : ScriptableObject
    {
        public AudioClip[] clips = new AudioClip[19];
        public AudioClip[] ambienceBeds;

        public AudioClip Get(Sfx s)
        {
            int i = (int)s;
            return (clips != null && i >= 0 && i < clips.Length) ? clips[i] : null;
        }
    }
}
