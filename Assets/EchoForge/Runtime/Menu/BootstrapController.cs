using System.Collections;
using UnityEngine;
using EchoForge.Game;
using EchoForge.Audio;

namespace EchoForge.Menu
{
    /// <summary>Entry scene: warm the save, apply audio settings, route to the Resonance Hall.</summary>
    public sealed class BootstrapController : MonoBehaviour
    {
        [SerializeField] float holdSeconds = 0.5f;

        IEnumerator Start()
        {
            _ = SaveSystem.Data;                 // force load / recovery
            AudioController.Instance?.ApplyVolumes();
            yield return new WaitForSeconds(holdSeconds);
            SceneFlow.LoadHall();
        }
    }
}
