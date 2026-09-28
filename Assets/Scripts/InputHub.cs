using UnityEngine;
using UnityEngine.InputSystem;
using DDD.EventSystem;

namespace DDD
{
    /// <summary>
    /// Central hub that listens to Unity's New Input System events 
    /// and exposes clean properties for other game systems to read.
    /// </summary>
    public class InputHub : MonoBehaviour
    {
        // ==========================================
        // INPUT PROPERTIES (Read-only for external scripts)
        // ==========================================
        public Vector2 Move { get; private set; }
        public bool Jump { get; private set; }

        // Reference to the PlayerInput component attached to this GameObject
        private PlayerInput input;

        private void OnEnable()
        {
            // Cache the component if it hasn't been found yet
            if (input == null)
            {
                input = GetComponent<PlayerInput>();
            }

            // Subscribe to the global input event broadcast
            input.onActionTriggered += OnAction;

            // Subscribe to the unified pause events
            EventBus.Subscribe<OnPauseEvent>(OnPause);
        }

        private void OnDisable()
        {
            // Unsubscribe when disabled to prevent memory leaks
            input.onActionTriggered -= OnAction;
            EventBus.Unsubscribe<OnPauseEvent>(OnPause);
        }

        /// <summary>
        /// Global callback triggered by the PlayerInput component whenever ANY action state changes.
        /// </summary>
        /// <param name="context">The contextual data of the specific action being executed.</param>
        public void OnAction(InputAction.CallbackContext context)
        {
            // Filter actions based on the action names configured in your .inputactions asset
            switch (context.action.name)
            {
                // Vector2 containing joystick coordinates, WASD vectors, or D-pad input
                case "Move":
                    Move = context.ReadValue<Vector2>();
                    break;

                // Treats the button press as a boolean. 
                // Checks > 0.5f to evaluate true when a button or trigger is pressed past the threshold.
                case "Jump":
                    Jump = context.ReadValue<float>() > 0.5f;
                    break;
            }
        }

        // ==========================================
        // ACTION MAP MANAGEMENT (Pause/Unpause)
        // ==========================================

        /// <summary>
        /// Disables gameplay inputs and switches the input focus to your UI controls.
        /// </summary>
        public void OnPause(OnPauseEvent ev)
        {
            if (ev.isPaused)
            {
                input.SwitchCurrentActionMap("UI");

                // FIX: Manually reset gameplay values! Switching action maps prevents 
                // the input system from telling this script that the player let go of the keys.
                ResetGameplayInputs();
            }
            else
            {
                input.SwitchCurrentActionMap("Player");
            }
        }

        /// <summary>
        /// Resets all gameplay variables back to their default states to prevent
        /// input freezing (like running in place) during state transitions.
        /// </summary>
        private void ResetGameplayInputs()
        {
            Move = Vector2.zero;
            Jump = false;

            // Clear future inputs here (e.g., IsAttacking = false;)
        }
    }
}
