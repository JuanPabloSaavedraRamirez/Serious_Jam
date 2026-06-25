using UnityEngine;

public class PlatformController : MonoBehaviour, IPlatformSurface
{
    [SerializeField] private PlatformType _platformType;

    [SerializeField] private bool _invertRotation;
    [SerializeField] private float _extraOffset;

    private PlatformData _platformData;
    private float _lastFinalAngle;
    private float _angularVelocityRad;
    private bool _hasLastAngle;

    private void Start()
    {
        PlatformManager.Source.OnTiltChanged += HandleTiltChanged;

        LoadPlatformData();
        HandleTiltChanged(PlatformManager.Source.CurrentAngle);
    }

    private void OnDestroy()
    {
        PlatformManager.Source.OnTiltChanged -= HandleTiltChanged;
    }

    private void LoadPlatformData()
    {
        if (PlatformManager.Source == null) return;

        bool dataRetrived = PlatformManager.Source.GetPlatformData(_platformType, out _platformData);
    }

    private void HandleTiltChanged(float globalAngle)
    {
        if (_platformData == null) return;

        float direction = _invertRotation ? -1f : 1f;

        float finalAngle = _platformData.BaseAnle + globalAngle * _platformData.AngleMultipler * direction + _extraOffset;

        finalAngle = Mathf.Clamp(finalAngle, _platformData.MinimumAngle, _platformData.MaximumAngle);
        UpdateAngularVelocity(finalAngle);

        transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
    }

    private void UpdateAngularVelocity(float finalAngle)
    {
        if (!_hasLastAngle)
        {
            _lastFinalAngle = finalAngle;
            _angularVelocityRad = 0f;
            _hasLastAngle = true;
            return;
        }

        float deltaTime = Time.deltaTime;

        if (deltaTime <= 0f)
        {
            _angularVelocityRad = 0f;
            return;
        }

        float deltaAngle = Mathf.DeltaAngle(_lastFinalAngle, finalAngle);
        float angularVelocityDeg = deltaAngle / deltaTime;

        _angularVelocityRad = angularVelocityDeg * Mathf.Deg2Rad;
        _lastFinalAngle = finalAngle;
    }

    public Vector3 GetVelocityAtPoint(Vector3 worldPoint)
    {
        Vector3 rotationAxis = transform.forward;
        Vector3 angularVelocity = rotationAxis * _angularVelocityRad;
        Vector3 radius = worldPoint - transform.position;

        return Vector3.Cross(angularVelocity, radius);
    }
}

public interface IPlatformSurface
{
    Vector3 GetVelocityAtPoint(Vector3 worldPoint);
}
