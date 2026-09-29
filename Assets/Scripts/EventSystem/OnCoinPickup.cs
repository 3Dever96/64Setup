using UnityEngine;

namespace DDD.EventSystem
{
    public struct OnCoinPickup : IEvent
    {
        public int value;

        public OnCoinPickup(int newValue)
        {
            value = newValue;
        }
    }
}
