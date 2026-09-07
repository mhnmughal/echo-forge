using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using EchoForge.Audio;

namespace EchoForge.EditorTools
{
    /// <summary>Offline procedural synthesis of every Echo Forge sound cue + an ambience bed.
    /// Writes 16-bit mono WAVs and assigns them to the SoundBank. No external audio, no
    /// network, no runtime generation. See Assets/Documentation/AUDIO_PROVENANCE.md.</summary>
    public static class AudioGen
    {
        const int SR = 44100;
        const string OutDir = "Assets/EchoForge/Audio/Generated";

        [MenuItem("Echo Forge/Build/5 - Generate Audio")]
        public static void Generate()
        {
            Directory.CreateDirectory(OutDir);

            Write("emitter_pulse", Emitter());
            Write("reflector_hit", Ping(1400f, 0.14f, 0.6f, metallic: true));
            Write("absorber_impact", Thud(140f, 0.22f));
            Write("splitter_branch", Concat(Ping(900f, 0.08f, 0.5f, true), Ping(1350f, 0.10f, 0.5f, true)));
            Write("delay_charge", Sweep(200f, 620f, 0.32f, 0.4f));
            Write("delay_release", Concat(Click(0.012f), Ping(520f, 0.14f, 0.5f, false)));
            Write("receiver_activate", Tone(233.08f, 0.36f, 0.5f, vibrato: 4f));
            Write("forbidden_receiver", Buzz(104f, 0.30f));
            Write("resonator", Chord(new[] { 174.61f, 261.63f }, 0.55f, 0.45f));
            Write("component_pickup", Click(0.05f, 0.3f));
            Write("component_socket", Concat(Click(0.02f, 0.35f), Ping(360f, 0.06f, 0.3f, false)));
            Write("component_rotate", Tick(1800f, 0.045f));
            Write("pulse_actuator", Concat(Click(0.015f, 0.5f), Thud(92f, 0.2f)));
            Write("resonance_dial", Tick(1200f, 0.05f, 0.25f));
            Write("pause_damping", Noise(0.5f, 0.35f, lowpassSweepDown: true));
            Write("settings_tick", Tick(2200f, 0.03f, 0.2f));
            Write("success", Arp(new[] { 261.63f, 329.63f, 392.0f, 523.25f }, 0.16f, 0.5f));
            Write("failure", Arp(new[] { 220.0f, 196.0f }, 0.22f, 0.5f));
            Write("final_resonance", ChordBloom(new[] { 130.81f, 196.0f, 261.63f, 392.0f }, 1.7f));
            Write("ambience_bed", Ambience(12f));

            AssetDatabase.Refresh();

            foreach (var f in Directory.GetFiles(OutDir, "*.wav"))
            {
                var imp = (AudioImporter)AssetImporter.GetAtPath(f.Replace('\\', '/'));
                if (imp == null) continue;
                imp.forceToMono = true;
                var ss = imp.defaultSampleSettings;
                ss.loadType = AudioClipLoadType.DecompressOnLoad;
                ss.compressionFormat = AudioCompressionFormat.Vorbis;
                ss.quality = 0.7f;
                imp.defaultSampleSettings = ss;
                imp.SaveAndReimport();
            }

            var bank = AssetDatabase.LoadAssetAtPath<SoundBank>($"{EchoBuild.ResDir}/SoundBank.asset");
            if (bank == null)
            {
                bank = ScriptableObject.CreateInstance<SoundBank>();
                AssetDatabase.CreateAsset(bank, $"{EchoBuild.ResDir}/SoundBank.asset");
            }
            bank.clips = new AudioClip[19];
            string[] names =
            {
                "emitter_pulse","reflector_hit","absorber_impact","splitter_branch","delay_charge",
                "delay_release","receiver_activate","forbidden_receiver","resonator","component_pickup",
                "component_socket","component_rotate","pulse_actuator","resonance_dial","pause_damping",
                "settings_tick","success","failure","final_resonance"
            };
            for (int i = 0; i < names.Length; i++)
                bank.clips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>($"{OutDir}/{names[i]}.wav");
            bank.ambienceBeds = new[] { AssetDatabase.LoadAssetAtPath<AudioClip>($"{OutDir}/ambience_bed.wav") };
            EditorUtility.SetDirty(bank);
            AssetDatabase.SaveAssets();

            Debug.Log("<b>Echo Forge: generated 20 original audio assets + assigned SoundBank.</b>");
        }

        // ---------------- synth primitives ----------------

        static float Env(float t, float dur, float atk, float rel)
        {
            if (t < atk) return t / atk;
            if (t > dur - rel) return Mathf.Max(0f, (dur - t) / rel);
            return 1f;
        }

        static float[] Tone(float freq, float dur, float amp, float vibrato = 0f)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float f = freq + (vibrato > 0 ? Mathf.Sin(t * vibrato * Mathf.PI * 2f) * freq * 0.01f : 0f);
                b[i] = Mathf.Sin(t * f * Mathf.PI * 2f) * amp * Env(t, dur, 0.02f, dur * 0.6f);
            }
            return b;
        }

        static float[] Ping(float freq, float dur, float amp, bool metallic)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            float[] parts = metallic ? new[] { 1f, 2.76f, 5.4f } : new[] { 1f, 2f };
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float s = 0f;
                for (int p = 0; p < parts.Length; p++)
                    s += Mathf.Sin(t * freq * parts[p] * Mathf.PI * 2f) / (p + 1);
                b[i] = s * amp * Mathf.Exp(-t * (metallic ? 26f : 34f));
            }
            return b;
        }

        static float[] Thud(float freq, float dur)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            var rnd = new System.Random(7);
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float sine = Mathf.Sin(t * freq * Mathf.PI * 2f * Mathf.Exp(-t * 3f));
                float noise = (float)(rnd.NextDouble() * 2 - 1) * 0.3f;
                b[i] = (sine + noise) * 0.6f * Mathf.Exp(-t * 18f);
            }
            return b;
        }

        static float[] Sweep(float f0, float f1, float dur, float amp)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            float phase = 0f;
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float f = Mathf.Lerp(f0, f1, t / dur);
                phase += f / SR * Mathf.PI * 2f;
                b[i] = Mathf.Sin(phase) * amp * Env(t, dur, 0.03f, 0.08f);
            }
            return b;
        }

        static float[] Buzz(float freq, float dur)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float saw = 2f * (t * freq - Mathf.Floor(0.5f + t * freq));
                float saw2 = 2f * (t * freq * 1.06f - Mathf.Floor(0.5f + t * freq * 1.06f));
                b[i] = (saw + saw2) * 0.22f * Env(t, dur, 0.01f, 0.15f);
            }
            return b;
        }

        static float[] Chord(float[] freqs, float dur, float amp)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float s = 0f;
                foreach (var f in freqs) s += Mathf.Sin(t * f * Mathf.PI * 2f);
                b[i] = s / freqs.Length * amp * Env(t, dur, 0.03f, dur * 0.7f);
            }
            return b;
        }

        static float[] ChordBloom(float[] freqs, float dur)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float s = 0f;
                for (int k = 0; k < freqs.Length; k++)
                {
                    float delay = k * 0.12f;
                    if (t < delay) continue;
                    float lt = t - delay;
                    s += Mathf.Sin(lt * freqs[k] * Mathf.PI * 2f) * Mathf.Exp(-lt * 1.1f);
                }
                b[i] = s / freqs.Length * 0.5f * Env(t, dur, 0.05f, dur * 0.5f);
            }
            return b;
        }

        static float[] Arp(float[] freqs, float noteDur, float amp)
        {
            float[] result = Array.Empty<float>();
            foreach (var f in freqs) result = Concat(result, Ping(f, noteDur, amp, false));
            return result;
        }

        static float[] Click(float dur, float amp = 0.6f)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            var rnd = new System.Random(3);
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                b[i] = (float)(rnd.NextDouble() * 2 - 1) * amp * Mathf.Exp(-t * 260f);
            }
            return b;
        }

        static float[] Tick(float freq, float dur, float amp = 0.4f)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                b[i] = Mathf.Sin(t * freq * Mathf.PI * 2f) * amp * Mathf.Exp(-t * 120f);
            }
            return b;
        }

        static float[] Noise(float dur, float amp, bool lowpassSweepDown)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            var rnd = new System.Random(11);
            float lp = 0f;
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float raw = (float)(rnd.NextDouble() * 2 - 1);
                float a = lowpassSweepDown ? Mathf.Lerp(0.4f, 0.02f, t / dur) : 0.2f;
                lp += a * (raw - lp);
                b[i] = lp * amp * Env(t, dur, 0.05f, dur * 0.5f);
            }
            return b;
        }

        static float[] Emitter()
        {
            int n = (int)(0.28f * SR);
            var b = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float f = Mathf.Lerp(420f, 170f, Mathf.Pow(t / 0.28f, 0.6f));
                b[i] = Mathf.Sin(t * f * Mathf.PI * 2f) * 0.5f * Env(t, 0.28f, 0.008f, 0.18f);
            }
            return b;
        }

        static float[] Ambience(float dur)
        {
            int n = (int)(dur * SR);
            var b = new float[n];
            var rnd = new System.Random(42);
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SR;
                float hum = Mathf.Sin(t * 58f * Mathf.PI * 2f) * 0.16f
                          + Mathf.Sin(t * 87f * Mathf.PI * 2f) * 0.10f
                          + Mathf.Sin(t * 116f * Mathf.PI * 2f) * 0.05f;
                float lfo = 0.75f + 0.25f * Mathf.Sin(t * 0.15f * Mathf.PI * 2f);
                float air = (float)(rnd.NextDouble() * 2 - 1) * 0.012f;
                // sparse tonal pulse every ~3s
                float ph = t % 3f;
                float bell = ph < 0.9f ? Mathf.Sin(t * 261.63f * Mathf.PI * 2f) * 0.06f * Mathf.Exp(-ph * 3.5f) : 0f;
                b[i] = (hum * lfo + air + bell);
            }
            // gentle loop crossfade
            int fade = (int)(0.25f * SR);
            for (int i = 0; i < fade; i++)
            {
                float k = i / (float)fade;
                b[i] *= k;
                b[n - 1 - i] *= k;
            }
            return b;
        }

        static float[] Concat(float[] a, float[] b)
        {
            var r = new float[a.Length + b.Length];
            Array.Copy(a, r, a.Length);
            Array.Copy(b, 0, r, a.Length, b.Length);
            return r;
        }

        // ---------------- WAV writer ----------------

        static void Write(string name, float[] samples)
        {
            // normalize to -1.5 dB
            float peak = 0.0001f;
            foreach (var s in samples) peak = Mathf.Max(peak, Mathf.Abs(s));
            float gain = 0.84f / peak;

            string path = $"{OutDir}/{name}.wav";
            using var fs = new FileStream(path, FileMode.Create);
            using var w = new BinaryWriter(fs);
            int dataBytes = samples.Length * 2;
            w.Write(new[] { 'R', 'I', 'F', 'F' });
            w.Write(36 + dataBytes);
            w.Write(new[] { 'W', 'A', 'V', 'E' });
            w.Write(new[] { 'f', 'm', 't', ' ' });
            w.Write(16);
            w.Write((short)1);
            w.Write((short)1);
            w.Write(SR);
            w.Write(SR * 2);
            w.Write((short)2);
            w.Write((short)16);
            w.Write(new[] { 'd', 'a', 't', 'a' });
            w.Write(dataBytes);
            foreach (var s in samples)
                w.Write((short)(Mathf.Clamp(s * gain, -1f, 1f) * 32767));
        }
    }
}
