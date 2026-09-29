using System.Collections.Generic;
using UnityEngine;
using DDD;

namespace DDD.Interactions
{
    /// <summary>
    /// Processes available runtime interactable objects, sorting them continuously by spatial proximity 
    /// and executing callbacks when inputs are processed.
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        // Reference to the centralized input processing system
        private InputHub input;

        // Tracks nearby interactable objects in memory
        private List<IInteractable> interactions = new List<IInteractable>();

        // Debounce flag to ensure an interaction triggers exactly once per keydown phase
        private bool canInteract;

        private IInteractable currentInteraction;
        private IInteractable lastInteraction;

        private void Start()
        {
            // Cache the local InputHub component on initialization
            input = GetComponent<InputHub>();
        }

        private void Update()
        {
            // Sort available interactables so the closest target is placed at index 0
            interactions.Sort(SortInteractions);

            // Fetch closest item if collection contains entries
            currentInteraction = interactions.Count > 0 ? interactions[0] : null;

            // Handle selection highlighting changes
            if (currentInteraction != lastInteraction)
            {
                if (lastInteraction != null)
                {
                    lastInteraction.IsCurrent = false;
                }

                if (currentInteraction != null)
                {
                    currentInteraction.IsCurrent = true;
                }

                lastInteraction = currentInteraction;
            }

            // If a valid interactable target exists, monitor button triggers
            if (currentInteraction != null)
            {
                // Ensure InputHub has a matching public bool "Interact" property configured
                if (input.Interact && canInteract)
                {
                    currentInteraction.OnInteract();
                }

                // Lock structural execution until button release
                canInteract = !input.Interact;
            }
        }

        /// <summary>
        /// Registers a nearby valid target to the potential tracking pool.
        /// </summary>
        public bool AddInteraction(IInteractable newInteraction)
        {
            if (!interactions.Contains(newInteraction))
            {
                interactions.Add(newInteraction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a target from the potential tracking pool when out of range.
        /// </summary>
        public bool RemoveInteraction(IInteractable newInteraction)
        {
            if (interactions.Contains(newInteraction))
            {
                // Safety cleanup: Ensure it drops its highlights immediately on exit
                newInteraction.IsCurrent = false;
                interactions.Remove(newInteraction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Comparison sort logic evaluating top-down flat distance measurements.
        /// </summary>
        private int SortInteractions(IInteractable a, IInteractable b)
        {
            // Calculate player's flat top-down coordinates
            Vector2 position = new Vector2(transform.position.x, transform.position.z);

            Vector2 posA = a.Position;
            Vector2 posB = b.Position;

            // Compute distance parameters
            float distA = Vector2.Distance(position, posA);
            float distB = Vector2.Distance(position, posB);

            // Return sorting weight structures
            if (distA > distB) return 1;
            if (distA < distB) return -1;
            return 0;
        }
    }
}
