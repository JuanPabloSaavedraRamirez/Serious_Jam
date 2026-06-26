using UnityEngine;

[CreateAssetMenu(fileName = "BallConfiguration", menuName = "Scriptable Objects/BallConfiguration")]
public class BallConfiguration : ScriptableObject
{
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _surfaceGravityMultiplier = 1f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _surfaceFriction = 1.5f;
    [SerializeField] private float _airDrag = 0.2f;

    public float Gravity => _gravity;
    public float SurfaceAcceleration => _surfaceGravityMultiplier;
    public float MaximumSpeed => _maxSpeed;
    public float SurfaceFriction => _surfaceFriction;
    public float AirDrag => _airDrag;
}
