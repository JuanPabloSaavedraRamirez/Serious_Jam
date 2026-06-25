using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Project/PC/InputReader", order = 0)]
    public class InputReader : ScriptableObject, GameInput.ISystemActions, GameInput.IUIActions, GameInput.IGameplayActions, GameInput.IPauseActions
    {
        private GameInput _gameInput;

        //Gameplay Events
        public event Action<Vector2> MovementEvent;
        public event Action PauseEvent;

        //Pause Events
        public event Action ResumePauseEvent;

        //UI Events
        public event Action<Vector2> NavigateEvent;
        public event Action SubmitPressedEvent;
        public event Action CancelPressedEvent;

        //System Events
        public event Action PressToContinueEvent;

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();

                _gameInput.System.SetCallbacks(this);
                _gameInput.UI.SetCallbacks(this);
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.Pause.SetCallbacks(this);
            }
        }

        private void OnDisable()
        {
            DisableInputs();
        }

        public void SetGameplayActionMap()
        {
            DisableInputs();
            _gameInput.Gameplay.Enable();

            Debug.LogWarning("Set to Gameplay");
        }

        public void SetPauseActionMap()
        {
            DisableInputs();
            _gameInput.Pause.Enable();

            Debug.LogWarning("Set to Pause");
        }

        public void SetUIActionMap()
        {
            DisableInputs();
            _gameInput.UI.Enable();

            Debug.LogWarning("Set to UI");
        }

        public void SetSystemActionMap()
        {
            DisableInputs();
            _gameInput.System.Enable();

            Debug.LogWarning("Set to System");
        }

        private void DisableInputs()
        {
            _gameInput.System.Disable();
            _gameInput.UI.Disable();
            _gameInput.Gameplay.Disable();
            _gameInput.Pause.Disable();
        }

        #region Gameplay Actions
        public void OnMove(InputAction.CallbackContext context)
        {
            MovementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent?.Invoke();
            }
        }
        #endregion

        #region Pause Actions
        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ResumePauseEvent?.Invoke();
            }
        }
        #endregion

        #region UI Actions
        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                NavigateEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                SubmitPressedEvent?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CancelPressedEvent?.Invoke();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {

            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {

            }
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {

            }
        }
        #endregion

        #region System Actions
        public void OnPressToContinue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                PressToContinueEvent?.Invoke();
            }
        }
        #endregion
    }
}

