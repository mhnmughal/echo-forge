using UnityEngine;
using UnityEngine.Audio;
using EchoForge.Game;

namespace EchoForge.Audio
{
    public enum Sfx
    {
        EmitterPulse, ReflectorHit, AbsorberImpact, SplitterBranch, DelayCharge, DelayRelease,
        ReceiverActivate, ForbiddenReceiver, Resonator, ComponentPickup, ComponentSocket,
        ComponentRotate, PulseActuator, ResonanceDial, PauseDamping, SettingsTick,
        Success, Failure, FinalResonance
    }

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

    /// <summary>Central non-networked audio. Mixer groups: Master / Ambience / SFX / UI.
    /// Volumes persist via SaveSystem. Cosmetic only - never affects simulation.</summary>
    [DefaultExecutionOrder(-50)]
    public sealed class AudioController : MonoBehaviour
    {
        public static AudioController Instance { get; private set; }

        [SerializeField] AudioMixer mixer;
        [SerializeField] AudioMixerGroup sfxGroup;
        [SerializeField] AudioMixerGroup uiGroup;
        [SerializeField] AudioMixerGroup ambienceGroup;
        [SerializeField] SoundBank bank;
        [SerializeField] AudioSource ambienceSource;

        AudioSource[] _pool;
        int _next;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _pool = new AudioSource[12];
            for (int i = 0; i < _pool.Length; i++)
            {
                var s = new GameObject($"sfx_{i}").AddComponent<AudioSource>();
                s.transform.SetParent(transform);
                s.playOnAwake = false;
                s.outputAudioMixerGroup = sfxGroup;
                _pool[i] = s;
            }
            ApplyVolumes();
        }

        public void ApplyVolumes()
        {
            SetDb("MusicVol", SaveSystem.Data.musicVolume);
            SetDb("EffectsVol", SaveSystem.Data.effectsVolume);
        }

        void SetDb(string param, float linear01)
        {
            if (mixer == null) return;
            float db = linear01 <= 0.0001f ? -80f : Mathf.Log10(Mathf.Clamp01(linear01)) * 20f;
            mixer.SetFloat(param, db);
        }

        public void Play(Sfx cue, float pitch = 1f, float volume = 1f)
        {
            var clip = bank != null ? bank.Get(cue) : null;
            if (clip == null) return;
            var src = _pool[_next]; _next = (_next + 1) % _pool.Length;
            src.pitch = pitch;
            src.PlayOneShot(clip, volume);
        }

        public void PlayAmbience()
        {
            if (ambienceSource == null || bank == null || bank.ambienceBeds == null || bank.ambienceBeds.Length == 0) return;
            ambienceSource.clip = bank.ambienceBeds[0];
            ambienceSource.loop = true;
            ambienceSource.outputAudioMixerGroup = ambienceGroup;
            ambienceSource.Play();
        }
    }
}
