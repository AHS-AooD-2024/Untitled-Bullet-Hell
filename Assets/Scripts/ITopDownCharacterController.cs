using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller that can move a character.
/// </summary>
public interface ITopDownCharacterController {
    /// <summary>
    /// Whether the controlled character is currently in a dash.
    /// </summary>
    public bool IsDashing { get; }

    /// <summary>
    /// The current velocity of the character.
    /// </summary>
    public Vector2 Velocity { get; }

    /// <summary>
    /// The direction the character is moving in. This is equivalent to
    /// the normalized velocity vector.
    /// </summary>
    public Vector2 Movement { get => Velocity.normalized; }

    /// <summary>
    /// The Speed at which the character moves.
    /// </summary>
    public float MovementSpeed { get; }

    /// <summary>
    /// The coefficient of speed for a dash interpolated for 
    /// <paramref name="t"/> across <c> [0, 1] </c>
    /// </summary>
    /// <param name="t">The progress of the dash from <c>[0, 1]</c>. 
    /// This is used as the parameter for the function of dash speed.</param>
    /// <returns>A speed coefficient, which is multiplied on top of MovementSpeed
    /// during a dash.</returns>
    public float DashSpeedCoefficient(float t);

    /// <summary>
    /// The duration of a dash in seconds.
    /// </summary>
    public float DashDurationSeconds { get; }

    /// <summary>
    /// The cooldown between dashes in seconds.
    /// </summary>
    public float DashCooldownSeconds { get; }

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// gaurentee that it will be given normalized.</param>
    public void Move(Vector2 movement) => Move(movement, false);

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// gaurentee that it will be given normalized.</param>
    /// <param name="doDash">If the character should dash.</param>
    public void Move(Vector2 movement, bool doDash) => Move(movement, doDash, Time.deltaTime);

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// gaurentee that it will be given normalized.</param>
    /// <param name="doDash">If the character should dash.</param>
    /// <param name="deltaTime">The time in seconds between individual calls 
    /// of <c>Move</c>. By default, this will be <see cref="Time.deltaTime"/></param>
    public void Move(Vector2 movement, bool doDash, float deltaTime) => Move(movement, Vector2.zero, doDash, deltaTime);

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// gaurentee that it will be given normalized.</param>
    /// <param name="look">The direction for the character to face. If this 
    /// is zero (or normalized to zero), the character will face the direction
    /// they were moving in.</param>
    /// <param name="doDash">If the character should dash.</param>
    public void Move(Vector2 movement, Vector2 look, bool doDash) => Move(movement, look, doDash, Time.deltaTime);

    /// <summary>
    /// Moves the character.
    /// </summary>
    /// <param name="movement">The direction for the character to move in. 
    /// Implementations should make sure to normalize this, as there is no 
    /// gaurentee that it will be given normalized.</param>
    /// <param name="look">The direction for the character to face. If this 
    /// is zero (or normalized to zero), the character will face the direction
    /// they were moving in.</param>
    /// <param name="doDash">If the character should dash.</param>
    /// <param name="deltaTime">The time in seconds between individual calls 
    /// of <c>Move</c>. By default, this will be <see cref="Time.deltaTime"/></param>
    public void Move(Vector2 movement, Vector2 look, bool doDash, float deltaTime);
}
