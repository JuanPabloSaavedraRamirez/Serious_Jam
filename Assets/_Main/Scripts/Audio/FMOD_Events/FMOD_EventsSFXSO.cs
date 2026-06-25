using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FMOD_EventsSFXSO", menuName = "Project/Audio/FMOD_Events/FMOD_EventsSFXSO")]
public class FMOD_EventsSFXSO : ScriptableObject
{
    #region UI
    [field: Header("UI")]
    [field: SerializeField] public EventReference ui_ButtonPressed { get; private set; }
    [field: SerializeField] public EventReference ui_ButtonConfirmPressed { get; private set; }
    [field: SerializeField] public EventReference UI_LoadingCompletedEvent { get; private set; }
    [field: SerializeField] public EventReference UI_LeftButtonPressedEvent { get; private set; }
    [field: SerializeField] public EventReference UI_RightButtonPressedEvent { get; private set; }
    #endregion
}
