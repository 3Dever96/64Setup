using UnityEngine;

namespace DDD.Characters.Player
{
    /// <summary>
    /// Base abstract class for all player state behaviors within the state machine.
    /// </summary>
    [System.Serializable]
    public abstract class PlayerState
    {
        /// <summary> Called once when the player transitions into this state. </summary>
        public abstract void StartState(PlayerController player);

        /// <summary> Called every frame to handle state-specific logic and movement input processing. </summary>
        public abstract void UpdateState(PlayerController player);

        /// <summary> Called every frame to check condition thresholds for shifting to another state. </summary>
        public abstract void ChangeState(PlayerController player);

        /// <summary> Called once when the player transitions out of this state. </summary>
        public abstract void ExitState(PlayerController player);
    }
}
