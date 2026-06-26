#if UNITY_EDITOR
using UnityEngine;

public class PlatformInput : MonoBehaviour
{
    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        PlatformManager.Source?.TiltPlatform(input);
    }
}
#endif