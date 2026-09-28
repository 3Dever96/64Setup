using UnityEngine;

namespace DDD.Characters.Player
{
    /// <summary>
    /// Handles player inputs, orientation adjustments, and jumps when the character is touching the ground.
    /// </summary>
    [System.Serializable]
    public class PlayerGroundState : PlayerState
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float stickForce; // Downward velocity to keep the player clamped to slopes
        [SerializeField] private float jumpSpeed;

        [Header("Collision Settings")]
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float groundCheckHeight;

        // Debounce flag ensuring the player must release jump before they can trigger it again
        private bool canJump;

        public override void StartState(PlayerController player)
        {
            // Apply a constant subtle downward force so the controller remains grounded on sloped terrain
            player.VerticalSpeed = stickForce;
            canJump = false;
        }

        public override void UpdateState(PlayerController player)
        {
            // Calculate movement vectors relative to the main camera's facing direction
            Vector3 direction = Camera.main.transform.right * player.Input.Move.x + Camera.main.transform.forward * player.Input.Move.y;
            direction.y = 0f; // Prevent moving vertically relative to camera pitch angle

            // Maintain previous facing direction if no movement input is provided
            player.Direction = direction != Vector3.zero ? direction.normalized : player.Direction;

            // Calculate current running velocity mapped dynamically to joystick pressure threshold
            player.CurrentSpeed = moveSpeed * player.Input.Move.magnitude;
            player.FaceDirection(player.Direction);

            // Execute jump mechanics if button is pressed and debounce condition is met
            if (player.Input.Jump && canJump)
            {
                player.VerticalSpeed = jumpSpeed;
            }

            // Lock structural jump execution until the input flag returns false (user releases the key)
            canJump = !player.Input.Jump;
        }

        public override void ChangeState(PlayerController player)
        {
            // Check if player has an upward velocity spike, or if a sphere cast fails to find solid objects beneath them
            if (player.VerticalSpeed > 0f || !Physics.CheckSphere(player.transform.position + Vector3.up * (groundCheckHeight), groundCheckRadius, LayerMask.GetMask("Solid")))
            {
                player.SetState(player.AirState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            // Optional: Implement ground particle clear or run audio clip stops here
        }
    }
}
