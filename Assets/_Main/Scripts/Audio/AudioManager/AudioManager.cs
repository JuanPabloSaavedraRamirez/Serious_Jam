using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Project/Audio/AudioManager")]
public class AudioManager : ScriptableObject
{
    public static AudioManager Instance;
    public FMOD_EventsSO fMODEventsSO;

    private Bus _masterBus, _musicBus, _ambienceBus, _sfxBus;

    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance _ambienceEventInstance;
    private EventInstance _musicEventInstance;

    public void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();

        if (Application.isPlaying)
        {
            RuntimeManager.StudioSystem.flushCommands();

            if (!RuntimeManager.IsInitialized)
            {
                Debug.LogError("[AudioManager] FMOD RuntimeManager is not initialized.");
                return;
            }

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

            InitiliazeMusic(fMODEventsSO.fMODEventsMusicSO.mainMusicEvent);
        }
    }

    public void OnDisable()
    {
        CleanUp();
    }

    #region ChangeVolume Sliders

    public float GetBusVolume(Bus _bus)
    {
        _bus.getVolume(out float volume);
        return volume;
    }

    public void ChangeMasterVolume(float newValue)
    {
        float _finalValue = Mathf.Clamp01(GetBusVolume(_masterBus) + newValue);
        _masterBus.setVolume(_finalValue);
    }

    public void ChangeMusicVolume(float newValue)
    {
        float _finalValue = Mathf.Clamp01(GetBusVolume(_musicBus) + newValue);
        _musicBus.setVolume(_finalValue);
    }

    public void ChangeSFXVolume(float newValue)
    {
        float _finalValue = Mathf.Clamp01(GetBusVolume(_sfxBus) + newValue);
        _sfxBus.setVolume(_finalValue);
    }

    public void ChangeAmbienceVolume(float newValue)
    {
        float _finalValue = Mathf.Clamp01(GetBusVolume(_ambienceBus) + newValue);
        _ambienceBus.setVolume(_finalValue);
    }

    public float ReturnMasterValue()
    {
        float _value = GetBusVolume(_masterBus);
        if (_value > 1f) _value = 1f;
        if (_value < 0) _value = 0f;
        return _value;
    }

    public float ReturnSFXValue()
    {
        float _value = GetBusVolume(_sfxBus);
        if (_value > 1f) _value = 1f;
        if (_value < 0) _value = 0f;
        return _value;
    }

    public float ReturnAmbienceValue()
    {
        float _value = GetBusVolume(_ambienceBus);
        if (_value > 1f) _value = 1f;
        if (_value < 0) _value = 0f;
        return _value;
    }

    public float ReturnMusicValue()
    {
        float _value = GetBusVolume(_musicBus);
        if (_value > 1f) _value = 1f;
        if (_value < 0) _value = 0f;
        return _value;
    }
    #endregion

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void PlayOneShotSFX(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void InitiliazeMusic(EventReference musicEventReference)
    {
        _musicEventInstance = CreateEventInstance(musicEventReference);
        _musicEventInstance.start();
    }

    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        _ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        _ambienceEventInstance.start();

        Debug.Log("Ambience Initialized");
    }

    /*public void SetAmbienceParameter(AmbienceState ambienceState)
    {
        _ambienceEventInstance.setParameterByName("AmbienceState", (float)ambienceState);
    }*/

    public void SetMusicParameter(MusicState newMusicState)
    {
        _musicEventInstance.setParameterByName("MusicState", (float)newMusicState);
    }

    /*public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }*/

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in _eventEmitters)
        {
            emitter.Stop();
        }
    }
}