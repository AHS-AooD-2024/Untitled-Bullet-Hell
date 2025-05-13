using UnityEngine;

public interface ITopDownCharacterController : ILookingGlass {
    /// <summary>
    /// The current velocity of the character.
    /// </summary>
    Vector2 Velocity { get; }

    /// <summary>
    /// The direction the character is moving in. This is equivalent to
    /// the normalized velocity vector.
    /// </summary>
    Vector2 Movement { get => Velocity.normalized; }


    float LookAngle { get => Vector2.SignedAngle(Vector2.up, Direction); }

    Vector3 LookRotation { get => Vector3.forward * LookAngle; }

    /// <summary>
    /// The Speed at which the character moves.
    /// </summary>
    float MovementSpeed { get; }

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// guarantee that it will be given normalized.</param>
    /// <param name="look">The direction to look towards. If this is 
    /// <see cref="Vector2.zero"/>, looking defaults to the direction last 
    /// moved in</param>
    /// <param name="deltaTime">The time, in seconds, since the last call
    /// to <see cref="Move"/></param>
    void Move(Vector2 movement, Vector2 look, float deltaTime);

    void Move(Vector2 movement, Vector2 look) => Move(movement, look, Time.deltaTime);
    void Move(Vector2 movement, float deltaTime) => Move(movement, Vector2.zero, deltaTime);
    void Move(Vector2 movement) => Move(movement, Vector2.zero, Time.deltaTime);

    void Stop();
}