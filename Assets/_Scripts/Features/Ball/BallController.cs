using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private BallConfiguration _ball;

    [Header("Platform Interaction")]
    [SerializeField] private float _surfaceVelocityInfluence = 1f;
    [SerializeField] private float _launchNormalSpeed = 1.5f;
    [SerializeField] private float _launchBoost = 0.75f;
    [SerializeField] private float _groundIgnoreDuration = 0.08f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private float _groundCheckExtraDistance = 0.08f;
    [SerializeField] private float _sphereCastRadiusMultiplier = 0.85f;

    [Header("Plane Lock")]
    [SerializeField] private bool _lockDepth = true;
    [SerializeField] private float _lockedZPosition;

    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;

    private Vector3 _velocity;

    private bool _isGrounded;
    private Vector3 _surfaceNormal;
    private Vector3 _surfacePoint;
    private IPlatformSurface _surfaceVelocityProvider;

    private float _sphereRadius;
    private float _groundIgnoreTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereRadius = GetScaledSphereRadius();

        LockBallPosition();
    }

    private void FixedUpdate()
    {
        _rigidbody.angularVelocity = Vector3.zero;

        UpdateGroundIgnoreTimer(Time.fixedDeltaTime);
        CheckSurface();
        ApplyPhysics(Time.fixedDeltaTime);
        MoveBall(Time.fixedDeltaTime);
    }

    private void LockBallPosition()
    {
        if (_lockDepth)
        {
            _lockedZPosition = transform.position.z;

            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void UpdateGroundIgnoreTimer(float deltaTime)
    {
        if (_groundIgnoreTimer <= 0f) return;

        _groundIgnoreTimer -= deltaTime;
    }

    private void CheckSurface()
    {
        _isGrounded = false;
        _surfaceVelocityProvider = null;
        _surfaceNormal = Vector3.up;
        _surfacePoint = transform.position;

        if (_groundIgnoreTimer > 0f) return;

        float castRadius = _sphereRadius * _sphereCastRadiusMultiplier;
        float castDistance = _sphereRadius + _groundCheckExtraDistance;

        bool hitSurface = Physics.SphereCast(transform.position, castRadius, Vector3.down, out RaycastHit hit, castDistance, _platformLayer, QueryTriggerInteraction.Ignore);

        if (!hitSurface) return;

        _isGrounded = true;
        _surfaceNormal = hit.normal;
        _surfacePoint = hit.point;

        _surfaceVelocityProvider = hit.collider.GetComponentInParent<IPlatformSurface>();
    }

    private void ApplyPhysics(float deltaTime)
    {
        if (_isGrounded)
        {
            ApplyGroundedPhysics(deltaTime);
        }
        else
        {
            ApplyAirPhysics(deltaTime);
        }

        if (_lockDepth)
        {
            _velocity.z = 0f;
        }

        _velocity = Vector3.ClampMagnitude(_velocity, _ball.MaximumSpeed);
    }

    private void ApplyGroundedPhysics(float deltaTime)
    {
        Vector3 surfaceVelocity = Vector3.zero;

        if (_surfaceVelocityProvider != null)
        {
            surfaceVelocity =
                _surfaceVelocityProvider.GetVelocityAtPoint(_surfacePoint) *
                _surfaceVelocityInfluence;
        }

        Vector3 relativeVelocity = _velocity - surfaceVelocity;
        Vector3 tangentVelocity = Vector3.ProjectOnPlane(relativeVelocity, _surfaceNormal);
        Vector3 gravity = Vector3.down * _ball.Gravity;
        Vector3 gravityAlongSurface = Vector3.ProjectOnPlane(gravity, _surfaceNormal);

        tangentVelocity += gravityAlongSurface * _ball.SurfaceAcceleration * deltaTime;

        tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero, _ball.SurfaceFriction * deltaTime);

        float relativeNormalSpeed = Vector3.Dot(relativeVelocity, _surfaceNormal );

        Vector3 normalVelocity = Vector3.zero;

        if (relativeNormalSpeed > 0f)
        {
            normalVelocity = _surfaceNormal * relativeNormalSpeed;
        }

        _velocity = surfaceVelocity + tangentVelocity + normalVelocity;

        PlatformLaunch(surfaceVelocity);
    }

    private void PlatformLaunch(Vector3 surfaceVelocity)
    {
        float platformNormalSpeed = Vector3.Dot(surfaceVelocity, _surfaceNormal);

        if (platformNormalSpeed < _launchNormalSpeed) return;

        _velocity += _surfaceNormal * platformNormalSpeed * _launchBoost;

        _groundIgnoreTimer = _groundIgnoreDuration;
        _isGrounded = false;
    }

    private void ApplyAirPhysics(float deltaTime)
    {
        _velocity += Vector3.down * _ball.Gravity * deltaTime;

        _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _ball.AirDrag * deltaTime);
    }

    private void MoveBall(float deltaTime)
    {
        Vector3 nextPosition = _rigidbody.position + _velocity * deltaTime;

        if (_lockDepth)
        {
            nextPosition.z = _lockedZPosition;
        }

        _rigidbody.MovePosition(nextPosition);
    }

    private float GetScaledSphereRadius()
    {
        Vector3 scale = transform.lossyScale;

        float maxScale = Mathf.Max(scale.x, scale.y, scale.z);

        return _sphereCollider.radius * maxScale;
    }
}
