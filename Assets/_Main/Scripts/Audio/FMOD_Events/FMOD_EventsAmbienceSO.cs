using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FMOD_EventsAmbienceSO", menuName = "Project/Audio/FMOD_Events/FMOD_EventsAmbienceSO")]
public class FMOD_EventsAmbienceSO : ScriptableObject
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference mainAmbienceEvent { get; private set; }
}