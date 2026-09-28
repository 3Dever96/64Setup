using System;
using System.Collections.Generic;

namespace DDD.EventSystem
{
    /// <summary>
    /// A simple, centralized Event Bus that facilitates decoupled communication 
    /// between components using the Publish-Subscribe pattern.
    /// </summary>
    public static class EventBus
    {
        /// <summary>
        /// Stores the list of subscriber delegates grouped by their specific event type.
        /// </summary>
        public static readonly Dictionary<Type, List<Delegate>> subs = new();

        /// <summary>
        /// Subscribes a handler method to a specific type of event.
        /// </summary>
        /// <typeparam name="T">The type of event to listen for. Must implement IEvent.</typeparam>
        /// <param name="handler">The callback action to execute when the event is published.</param>
        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            Type t = typeof(T);

            // If the event type doesn't have a subscriber list yet, initialize it
            if (!subs.TryGetValue(t, out List<Delegate> list))
            {
                list = new();
                subs[t] = list;
            }

            // Add the handler delegate to the subscription list
            list.Add(handler);
        }

        /// <summary>
        /// Unsubscribes a handler method from a specific type of event.
        /// </summary>
        /// <typeparam name="T">The type of event to stop listening to. Must implement IEvent.</typeparam>
        /// <param name="handler">The callback action to remove.</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            Type t = typeof(T);

            // Check if a subscription list exists for this event type
            if (subs.TryGetValue(t, out List<Delegate> list))
            {
                list.Remove(handler);

                // Clean up the dictionary entry if there are no subscribers left
                if (list.Count == 0) subs.Remove(t);
            }
        }

        /// <summary>
        /// Broadcasts an event to all registered subscribers of that specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event being published. Must implement IEvent.</typeparam>
        /// <param name="ev">The event instance containing the payload data.</param>
        public static void Publish<T>(T ev) where T : IEvent
        {
            Type t = typeof(T);

            // Check if there are any active subscribers for this event type
            if (subs.TryGetValue(t, out List<Delegate> list))
            {
                // Copy to an array to prevent "Collection was modified" exceptions 
                // if a subscriber un-subscribes mid-execution.
                Delegate[] copy = list.ToArray();

                foreach (Delegate d in copy)
                {
                    // Cast the generic Delegate back to its strongly-typed Action and invoke it
                    ((Action<T>)d)(ev);
                }
            }
        }
    }

}
