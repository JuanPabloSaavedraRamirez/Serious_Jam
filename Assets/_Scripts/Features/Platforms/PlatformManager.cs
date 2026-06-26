using System;
using UnityEngine;

public class PlatformManager : Singleton<IPlatformSource>, IPlatformSource
{
    public event Action<float> OnTiltChanged;

    [Header("Platform Database")]
    [SerializeField] private PlatformDatabase _platformDatabase;

    [Header("Tilt Settings")]
    [SerializeField] private float _tiltSpeed = 90f;
    [SerializeField] private float _returnSpeed = 120f;
    [SerializeField] private float _maxGlobalAngle = 45f;

    [Header("Input Settings")]
    [SerializeField] private float _deadZone = 0.01f;

    public float CurrentAngle { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (_hasBeenDestroyed) return;

        _platformDatabase?.InitializeData();
    }

    public void TiltPlatform(float input)
    {
        if (Mathf.Abs(input) > _deadZone)
        {
            TiltByInput(input);
        }
        else
        {
            ResetPlatform();
        }

        OnTiltChanged?.Invoke(CurrentAngle);
    }

    public bool GetPlatformData(PlatformType platformType, out PlatformData rotationData)
    {
        rotationData = null;

        if (_platformDatabase == null)
        {
            return false;
        }

        return _platformDatabase.GetPlatformData(platformType, out rotationData);
    }

    private void TiltByInput(float input)
    {
        CurrentAngle += input * _tiltSpeed * Time.deltaTime;

        CurrentAngle = Mathf.Clamp(CurrentAngle, -_maxGlobalAngle, _maxGlobalAngle);
    }

    private void ResetPlatform()
    {
        CurrentAngle = Mathf.MoveTowards(CurrentAngle, 0f, _returnSpeed * Time.deltaTime);
    }
}

public interface IPlatformSource
{
    event Action<float> OnTiltChanged;
    
    float CurrentAngle {  get; }

    void TiltPlatform(float input);
    bool GetPlatformData(PlatformType platformType, out PlatformData platformData);
}

public enum PlatformType
{
    Flat,
    Triangle,
    Star
}
