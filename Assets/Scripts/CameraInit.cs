using UnityEngine;
using DDD.EventSystem;
using Unity.Cinemachine;

namespace DDD
{
    /// <summary>
    /// Listens for the player spawning event and dynamically configures 
    /// the Cinemachine camera to track and look at the new player instance.
    /// </summary>
    public class CameraInit : MonoBehaviour
    {
        // Reference to the active Cinemachine Camera component
        private CinemachineCamera cam;

        private void Awake()
        {
            // Cache the Cinemachine component attached to this GameObject
            cam = GetComponent<CinemachineCamera>();
        }

        private void OnEnable()
        {
            // Begin listening for the player spawn broadcast
            EventBus.Subscribe<OnPlayerSpawnEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            // Clean up subscription to prevent memory leaks
            EventBus.Unsubscribe<OnPlayerSpawnEvent>(OnPlayerSpawned);
        }

        /// <summary>
        /// Callback triggered automatically when a player spawns into the scene.
        /// </summary>
        /// <param name="ev">Event payload containing the spawned player's Transform.</param>
        private void OnPlayerSpawned(OnPlayerSpawnEvent ev)
        {
            // Safety check: Ensure the camera component was successfully cached
            if (cam == null)
            {
                Debug.LogError($"[CameraInit] CinemachineCamera component is missing on {gameObject.name}!", this);
                return;
            }

            // Assign the player's transform directly to the camera tracking channels
            cam.LookAt = ev.player;
            cam.Follow = ev.player;
        }
    }
}
