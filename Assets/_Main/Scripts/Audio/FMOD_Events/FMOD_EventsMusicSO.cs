using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FMOD_EventsMusicSO", menuName = "Project/Audio/FMOD_Events/FMOD_EventsMusicSO")]
public class FMOD_EventsMusicSO : ScriptableObject
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference mainMusicEvent { get; private set; }
}