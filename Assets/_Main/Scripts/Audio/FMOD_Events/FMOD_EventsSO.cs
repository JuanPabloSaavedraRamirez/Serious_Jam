using UnityEngine;

[CreateAssetMenu(fileName = "FMOD_EventsSO", menuName = "Project/Audio/FMOD_Events/FMOD_EventsSO")]
public class FMOD_EventsSO : ScriptableObject
{
    public FMOD_EventsMusicSO fMODEventsMusicSO;
    public FMOD_EventsSFXSO fMODEventsSFXSO;
    public FMOD_EventsAmbienceSO fMODEventsAmbienceSO;
}

