using UnityEngine;
using DDD.EventSystem;

namespace DDD.Interactables
{
    /// <summary>
    /// A physical item pickup that triggers an interaction event when a player enters its collision volume.
    /// automatically cleans itself up after being collected.
    /// </summary>
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
    public class CoinPickup : MonoBehaviour, IInteractable
    {
        public Vector2 Position { get { return new Vector2(transform.position.x, transform.position.z); } }
        public bool IsCurrent { get; set; }

        // The financial value or point worth of this specific coin instance
        [SerializeField] private int value;

        /// <summary>
        /// Executes the pickup logic, broadcasting the collection data to the global EventBus.
        /// </summary>
        public void OnInteract()
        {
            // Publish the event so score tracking systems or UIs can respond to the value change
            EventBus.Publish<OnCoinPickup>(new OnCoinPickup(value));

            // Remove the coin from the scene to prevent double collection
            Destroy(gameObject);
        }

        /// <summary>
        /// Native Unity callback triggered when another collider enters this trigger zone.
        /// </summary>
        /// <param name="other">The collider that entered the trigger volume.</param>
        private void OnTriggerEnter(Collider other)
        {
            // For safety, you might want to check if the colliding object is the player:
            // if (other.CompareTag("Player"))

            OnInteract();
        }
    }
}
