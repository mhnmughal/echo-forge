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

    /// <summary>Central non-networked audio. If an AudioMixer is assigned its exposed params
    /// MusicVol / EffectsVol are driven; otherwise category volume multipliers are applied
    /// directly. Volumes persist via SaveSystem. Cosmetic only - never affects simulation.</summary>
    [DefaultExecutionOrder(-50)]
    public sealed class AudioController : MonoBehaviour
    {
        public static AudioController Instance { get; private set; }

        [SerializeField] AudioMixer mixer;
        [SerializeField] AudioMixerGroup sfxGroup;
        [SerializeField] AudioMixerGroup ambienceGroup;
        [SerializeField] SoundBank bank;
        [SerializeField] AudioSource ambienceSource;

        AudioSource[] _pool;
        int _next;
        float _effects = 1f;
        bool _damped;

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
                if (sfxGroup != null) s.outputAudioMixerGroup = sfxGroup;
                _pool[i] = s;
            }
        }

        void Start() => ApplyVolumes();

        public void SetDamped(bool damped) { _damped = damped; ApplyVolumes(); }

        public void ApplyVolumes()
        {
            float music = Mathf.Clamp01(SaveSystem.Data.musicVolume);
            _effects = Mathf.Clamp01(SaveSystem.Data.effectsVolume);

            if (mixer != null)
            {
                mixer.SetFloat("MusicVol", ToDb(music));
                mixer.SetFloat("EffectsVol", ToDb(_effects));
            }
            if (ambienceSource != null)
                ambienceSource.volume = (mixer == null ? music : 1f) * (_damped ? 0.3f : 1f);
        }

        static float ToDb(float linear01) =>
            linear01 <= 0.0001f ? -80f : Mathf.Log10(linear01) * 20f;

        public void Play(Sfx cue, float pitch = 1f, float volume = 1f)
        {
            var clip = bank != null ? bank.Get(cue) : null;
            if (clip == null) return;
            var src = _pool[_next]; _next = (_next + 1) % _pool.Length;
            src.pitch = pitch;
            src.PlayOneShot(clip, volume * (mixer != null ? 1f : _effects));
        }

        public void PlayAmbience()
        {
            if (ambienceSource == null || bank == null || bank.ambienceBeds == null || bank.ambienceBeds.Length == 0)
                return;
            if (ambienceSource.isPlaying) return;
            ambienceSource.clip = bank.ambienceBeds[0];
            ambienceSource.loop = true;
            if (ambienceGroup != null) ambienceSource.outputAudioMixerGroup = ambienceGroup;
            ApplyVolumes();
            ambienceSource.Play();
        }
    }
}
