using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace EchoForge.Chamber
{
    public sealed class SocketMarker : MonoBehaviour
    {
        public Vector2Int Cell;
    }

    /// <summary>Touch-first pointer input for the chamber. One tap = one intent.
    /// Raycasts the board; routes to component / socket / background.</summary>
    public sealed class ChamberInput : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] LayerMask rayMask = ~0;

        public bool InputEnabled = true;

        public event Action<ComponentView> ComponentTapped;
        public event Action<Vector2Int> SocketTapped;
        public event Action BackgroundTapped;

        public void Configure(Camera c) => cam = c != null ? c : Camera.main;

        void Awake() { if (cam == null) cam = Camera.main; }

        void Update()
        {
            if (!InputEnabled || cam == null) return;

            var p = Pointer.current;
            if (p == null || !p.press.wasPressedThisFrame) return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 screen = p.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(screen);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, rayMask, QueryTriggerInteraction.Collide))
            {
                var view = hit.collider.GetComponentInParent<ComponentView>();
                if (view != null) { ComponentTapped?.Invoke(view); return; }

                var socket = hit.collider.GetComponentInParent<SocketMarker>();
                if (socket != null) { SocketTapped?.Invoke(socket.Cell); return; }
            }
            BackgroundTapped?.Invoke();
        }
    }
}
