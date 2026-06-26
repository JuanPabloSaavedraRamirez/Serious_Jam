using System;

namespace UI
{
    public class GUIEvents
    {
        #region Singleton
        private GUIEvents() { }
        public static GUIEvents Instance { get; private set; } = new GUIEvents();
        #endregion

        #region Events
        public event Action OnGameFailed;
        public event Action PlayerPaused;
        public event Action PlayerResumed;

        public void InvokePlayerPaused() => PlayerPaused?.Invoke();
        public void InvokePlayerResumed() => PlayerResumed?.Invoke();
        #endregion
    }
}