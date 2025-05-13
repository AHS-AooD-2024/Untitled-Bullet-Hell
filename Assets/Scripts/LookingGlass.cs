using UnityEngine;

/// <summary>
/// A monobehaviour that can tell where an object is looking
/// </summary>
public abstract class LookingGlass : MonoBehaviour, ILookingGlass {
    public abstract Vector2 Direction { get; }

    public float Angle { get => Vector2.Angle(Direction, Vector2.up); }
    public float SignedAngle { get => Vector2.SignedAngle(Direction, Vector2.up); }

    public Vector3 EulerAngles { get => Vector3.forward * SignedAngle; }
}