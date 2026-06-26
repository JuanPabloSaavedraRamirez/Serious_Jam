using Scripts.Input;
using UnityEngine;

namespace UI
{
    public class GUI_Pause : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pauseScreen;
        [SerializeField] private InputReader _inputReader;

        private PauseLogic _pauseLogic;
        private GUIEvents _guiEvents;

        #region Initialization
        private void Awake()
        {
            _guiEvents = GUIEvents.Instance;
        }

        private void Start()
        {
            _pauseLogic = new PauseLogic(_inputReader, GUIEvents.Instance);

            _guiEvents.PlayerPaused  += OnPlayerPaused;
            _guiEvents.PlayerResumed += OnPlayerResumed;
        }
        #endregion

        #region Cleanup
        private void OnDestroy()
        {
            _guiEvents.PlayerPaused -= OnPlayerPaused;
            _guiEvents.PlayerResumed -= OnPlayerResumed;
            _pauseLogic.Dispose();
        }
        #endregion

        #region Pause Logic
        private void OnPlayerPaused() => _pauseScreen.SetActive(true);
        private void OnPlayerResumed() => _pauseScreen.SetActive(false);
        #endregion

        //Needed Add logic to resume game and Back to main menu
    }
}