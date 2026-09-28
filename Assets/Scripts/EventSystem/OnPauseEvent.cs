namespace DDD.EventSystem
{
    /// <summary>
    /// Event payload broadcasted whenever the game is paused or unpaused.
    /// </summary>
    public struct OnPauseEvent : IEvent
    {
        /// <summary>
        /// True if the game is transitioning into a paused state; false if resuming gameplay.
        /// </summary>
        public bool isPaused;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnPauseEvent"/> struct.
        /// </summary>
        /// <param name="_isPaused">The target pause state of the game loop.</param>
        public OnPauseEvent(bool _isPaused)
        {
            isPaused = _isPaused;
        }
    }
}
