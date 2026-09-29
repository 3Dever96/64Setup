using UnityEngine;
using UnityEngine.Events;

namespace DDD.Interactions
{
    /// <summary>
    /// An environment interactable switch that can be toggled by player proximity inputs, 
    /// executing UnityEvent responses based on its internal state.
    /// </summary>
    public class Switch : MonoBehaviour, IInteractable
    {
        // Concrete interface properties returning a flat 2D projection profile
        public Vector2 Position { get; }
        public bool IsCurrent { get; set; }

        [Header("Event Triggers")]
        [SerializeField] private UnityEvent OnOn;
        [SerializeField] private UnityEvent OnOff;

        [Header("Status Configuration")]
        [SerializeField] private bool isOn;

        /// <summary>
        /// Toggles the activation state of the object and triggers corresponding target UnityEvents.
        /// </summary>
        public void OnInteract()
        {
            if (isOn)
            {
                isOn = false;
                OnOff?.Invoke();
            }
            else
            {
                isOn = true;
                OnOn?.Invoke();
            }
        }

        // Using OnTriggerEnter or OnTriggerStay to continuously check zone inclusions
        private void OnTriggerStay(Collider other)
        {
            InteractionManager manager = other.GetComponent<InteractionManager>();

            if (manager != null)
            {
                manager.AddInteraction(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            InteractionManager manager = other.GetComponent<InteractionManager>();

            if (manager != null)
            {
                manager.RemoveInteraction(this);
            }
        }
    }
}
