using Scripts.Input;

namespace UI
{
    public class PauseLogic
    {
        private readonly InputReader _inputReader;
        private readonly GUIEvents _guiEvents;

        #region Constructor
        public PauseLogic(InputReader inputReader, GUIEvents guiEvents)
        {
            _inputReader = inputReader;
            _guiEvents = guiEvents;

            _inputReader.PauseEvent += OnPause;
            _inputReader.ResumePauseEvent += OnResume;
        }
        #endregion

        #region Public Methods
        public void OnPause()
        {
            if (_inputReader != null)
                _inputReader.SetUIActionMap();
            if (_guiEvents != null)
                _guiEvents?.InvokePlayerPaused();
        }

        public void OnResume()
        {
            if (_inputReader != null)
                _inputReader.SetGameplayActionMap();
            if (_guiEvents != null)
                _guiEvents?.InvokePlayerResumed();
        }

        public void Dispose()
        {
            _inputReader.PauseEvent -= OnPause;
            _inputReader.ResumePauseEvent -= OnResume;
        }
        #endregion
    }
}