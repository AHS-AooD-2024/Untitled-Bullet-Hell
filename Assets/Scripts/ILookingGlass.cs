using UnityEngine;

public interface ILookingGlass {
    Vector2 Direction { get; }
    float Angle { get; }
    float SignedAngle { get; }
    Vector3 EulerAngles { get; }
}
