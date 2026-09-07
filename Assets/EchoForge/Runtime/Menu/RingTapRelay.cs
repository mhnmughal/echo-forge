using UnityEngine;
using UnityEngine.InputSystem;

namespace EchoForge.Menu
{
    /// <summary>Forwards a tap on the Suspended Resonance Ring collider to the hall controller.</summary>
    [RequireComponent(typeof(Collider))]
    public sealed class RingTapRelay : MonoBehaviour
    {
        [SerializeField] ResonanceHallController hall;
        Collider _col;

        void Awake() => _col = GetComponent<Collider>();

        void Update()
        {
            var p = Pointer.current;
            if (p == null || !p.press.wasPressedThisFrame || hall == null) return;
            var cam = Camera.main;
            if (cam == null) return;
            var ray = cam.ScreenPointToRay(p.position.ReadValue());
            if (_col.Raycast(ray, out _, 500f)) hall.OnRingTapped();
        }
    }
}
