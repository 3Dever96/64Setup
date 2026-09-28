using UnityEngine;
using DDD.EventSystem;

namespace DDD.Characters.Player
{
    /// <summary>
    /// Master controller for the player character. Manages state machine transitions, 
    /// component dependencies, and executes final character physics movement loops.
    /// </summary>
    [RequireComponent(typeof(InputHub)), RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        // ==========================================
        // COMPONENT & STATE REFERENCES
        // ==========================================
        public CharacterController Controller { get; private set; }
        public InputHub Input { get; private set; }

        public PlayerState CurrentState { get; private set; }

        [field: SerializeField] public PlayerGroundState GroundState { get; private set; } = new PlayerGroundState();
        [field: SerializeField] public PlayerAirState AirState { get; private set; } = new PlayerAirState();

        // ==========================================
        // RUNTIME MOVEMENT VARIABLES (Shared with States)
        // ==========================================
        public float CurrentSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public Vector3 Direction { get; set; }

        private void Start()
        {
            // Cache essential local engine and custom components
            Controller = GetComponent<CharacterController>();
            Input = GetComponent<InputHub>();

            // Broadcast to the system that the player has loaded so cameras/managers can target them
            EventBus.Publish<OnPlayerSpawnEvent>(new OnPlayerSpawnEvent(transform));

            // Default the player to the ground sequence on instantiation
            SetState(GroundState);
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                // Run the current active state's physics calculations and input evaluations
                CurrentState.UpdateState(this);

                // Evaluate conditions to check if a state swap should occur
                CurrentState.ChangeState(this);

                // Process and pass calculated values to the engine's CharacterController
                ApplyMovement();
            }
        }

        /// <summary>
        /// Safely transitions the state machine from the current behavior pattern into a new one.
        /// </summary>
        public void SetState(PlayerState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.ExitState(this);
            }

            CurrentState = newState;

            if (CurrentState != null)
            {
                CurrentState.StartState(this);
            }
        }

        /// <summary>
        /// Gradually rotates the player model toward a specific movement path trajectory.
        /// </summary>
        public void FaceDirection(Vector3 newDirection, float turnSpeed = 500f)
        {
            if (newDirection == Vector3.zero) return;

            // Interpolate standard rotation towards the target direction over time
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newDirection), turnSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Combines horizontal tracking direction and vertical gravitational speed to move the character.
        /// </summary>
        private void ApplyMovement()
        {
            Vector3 velocity = Direction * CurrentSpeed;
            velocity.y = VerticalSpeed;

            // Frame-rate independent translation call utilizing Unity standard velocity steps
            Controller.Move(velocity * Time.deltaTime);
        }
    }
}
