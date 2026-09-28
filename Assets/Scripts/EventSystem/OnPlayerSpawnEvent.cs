using UnityEngine;

namespace DDD.EventSystem
{
    /// <summary>
    /// Event payload broadcasted immediately after the player character spawns into the scene.
    /// Used by systems like the Virtual Camera to dynamically target the new player instance.
    /// </summary>
    public struct OnPlayerSpawnEvent : IEvent
    {
        /// <summary>
        /// The Transform component of the newly instantiated player GameObject.
        /// </summary>
        public Transform player;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnPlayerSpawnEvent"/> struct.
        /// </summary>
        /// <param name="newPlayer">The Transform of the spawned player character.</param>
        public OnPlayerSpawnEvent(Transform newPlayer)
        {
            player = newPlayer;
        }
    }
}
