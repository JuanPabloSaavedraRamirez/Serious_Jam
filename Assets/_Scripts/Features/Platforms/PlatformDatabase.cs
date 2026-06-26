using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformDatabase", menuName = "Scriptable Objects/PlatformDatabase")]
public class PlatformDatabase : ScriptableObject
{
    [SerializeField] private List<PlatformData> _platforms = new();

    private Dictionary<PlatformType, PlatformData> _platformData;

    public void InitializeData()
    {
        _platformData = new Dictionary<PlatformType, PlatformData>();

        for (int i = 0; i < _platforms.Count; i++)
        {
            PlatformData platform = _platforms[i];

            if (platform == null) continue;

            if (_platformData.ContainsKey(platform.Type)) continue;

            _platformData.Add(platform.Type, platform);
        }
    }

    public bool GetPlatformData(PlatformType type, out PlatformData data)
    {
        if ( _platformData == null)
        {
            InitializeData();
        }

        return _platformData.TryGetValue(type, out data);
    }
}
