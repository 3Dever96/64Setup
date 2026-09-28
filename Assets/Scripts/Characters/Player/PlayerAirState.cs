using UnityEngine;

namespace DDD.Characters.Player
{
    /// <summary>
    /// Handles air time logic including progressive gravity drop rates, ceiling collisions, and ground detection.
    /// </summary>
    [System.Serializable]
    public class PlayerAirState : PlayerState
    {
        [Header("Physics Settings")]
        [SerializeField] private float gravity;       // Usually a negative number (e.g., -9.81)
        [SerializeField] private float fallSpeed;     // Terminal velocity limit threshold

        [Header("Ceiling Collision Settings")]
        [SerializeField] private float headHeight;
        [SerializeField] private float groundCheckHeight;
        [SerializeField] private float collisionRadius;

        public override void StartState(PlayerController player)
        {

        }

        public override void UpdateState(PlayerController player)
        {
            player.FaceDirection(player.Direction);

            // CEILING CHECK / JUMP CANCEL:
            // Stops upward momentum instantly if player lets go of jump early OR triggers a head-bonk against a "Solid" layer
            if (!player.Input.Jump || Physics.CheckSphere(player.transform.position + Vector3.up * headHeight, collisionRadius, LayerMask.GetMask("Solid")))
            {
                player.VerticalSpeed = Mathf.Min(0f, player.VerticalSpeed);
            }

            // Apply incremental frame-rate adjusted gravity pull down to terminal velocity limit threshold
            if (player.VerticalSpeed > fallSpeed)
            {
                player.VerticalSpeed += gravity * Time.deltaTime;
            }
        }

        public override void ChangeState(PlayerController player)
        {
            // LANDING CHECK: 
            // If dropping downward, check a sphere layout below feet to find solid ground and return to ground state
            if (player.VerticalSpeed < 0f && Physics.CheckSphere(player.transform.position + Vector3.up * groundCheckHeight, collisionRadius, LayerMask.GetMask("Solid")))
            {
                player.SetState(player.GroundState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            // Optional: Implement landing squish effects or ground landing audio cues here
        }
    }
}
