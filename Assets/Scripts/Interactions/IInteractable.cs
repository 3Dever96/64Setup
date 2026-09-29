using UnityEngine;

/// <summary>
/// Defines a contract for any game object that can be interacted with by a player or agent.
/// </summary>
public interface IInteractable
{
    /// <summary> The 2D top-down positional coordinate used for distance proximity calculations. </summary>
    public Vector2 Position { get; }

    /// <summary> Tracks whether this is the absolute closest interactable object to the player. </summary>
    public bool IsCurrent { get; set; }

    /// <summary> Triggered when an interaction event occurs with this object. </summary>
    public void OnInteract();
}
