using UnityEngine;
using EchoForge.Sim;

namespace EchoForge.Chamber
{
    /// <summary>Scene-space representation of one acoustic component. Visual/interaction only -
    /// the authoritative state lives in ChamberRuntime. LevelController drives Apply().</summary>
    [RequireComponent(typeof(Collider))]
    public sealed class ComponentView : MonoBehaviour
    {
        public CompType Type { get; private set; }
        public int AdjustableIndex { get; private set; } = -1;
        public bool IsAdjustable => AdjustableIndex >= 0;
        public int ReceiverId { get; private set; }
        public bool Forbidden { get; private set; }

        [SerializeField] Transform spinner;      // rotates for reflector / splitter
        [SerializeField] GameObject selectionRing;
        [SerializeField] Renderer accent;        // receiver/resonator state glow

        static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        MaterialPropertyBlock _mpb;

        public Vector2Int Cell { get; private set; }

        public void Init(CompType type, int adjustableIndex, int receiverId, bool forbidden, Vector2Int cell)
        {
            Type = type;
            AdjustableIndex = adjustableIndex;
            ReceiverId = receiverId;
            Forbidden = forbidden;
            Cell = cell;
            SetSelected(false);
            SetActivated(false);
        }

        /// <summary>Runtime visual binding (board is spawned from level data, not a prefab yet).</summary>
        public void BindVisuals(Transform spinnerT, GameObject ring, Renderer accentR)
        {
            spinner = spinnerT;
            selectionRing = ring;
            accent = accentR;
        }

        public void ApplyOrientation(int orient)
        {
            if (spinner == null) return;
            float yaw = Type == CompType.Reflector
                ? orient switch { 0 => 45f, 1 => 135f, 2 => 90f, _ => 0f }
                : orient * 45f;
            spinner.localRotation = Quaternion.Euler(0f, yaw, 0f);
        }

        public void SetSelected(bool on)
        {
            if (selectionRing != null) selectionRing.SetActive(on);
        }

        public void SetActivated(bool on)
        {
            if (accent == null) return;
            _mpb ??= new MaterialPropertyBlock();
            accent.GetPropertyBlock(_mpb);

            bool receiverLike = Type == CompType.Receiver || Type == CompType.Resonator || Type == CompType.Emitter;
            Color baseC = Forbidden ? new Color(0.72f, 0.10f, 0.14f) : new Color(0.92f, 0.58f, 0.22f);
            // receivers/resonators/emitters keep a soft resting glow; everything brightens on a hit
            float mul = on ? 3.0f : (receiverLike ? 0.9f : 0f);
            _mpb.SetColor(EmissionColor, baseC * mul);
            accent.SetPropertyBlock(_mpb);
        }
    }
}
