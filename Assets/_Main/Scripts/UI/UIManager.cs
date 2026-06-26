using UnityEngine;
using System.Collections.Generic;
using UI.Menu;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Menu Data")]
        [SerializeField] private List<MenuData> _menuDataList;

        [Header("Splash Screen")]
        [SerializeField] private GameObject _splashScreen;
        private Button _splashScreenButton;

        private Dictionary<MenuData, UnityAction> _listenerCache = new();

        #region Subscriptions and Unsubscriptions
        private void OnEnable()
        {
            foreach (var menu in _menuDataList)
            {
                var captured = menu;
                UnityAction action = () => OpenMenu(captured.menuType);
                _listenerCache[menu] = action;
                menu.activateButton.onClick.AddListener(action);
            }
            _splashScreenButton = _splashScreen.GetComponent<Button>();
            
            if (_splashScreenButton != null) _splashScreenButton.onClick.AddListener(StartMenu);
            else Debug.LogWarning("[UIManager] No Button component found on the Splash Screen GameObject.");
        }
        private void OnDisable()
        {
            foreach (var menu in _menuDataList)
            {
                if (_listenerCache.TryGetValue(menu, out var action))
                    menu.activateButton.onClick.RemoveListener(action);
            }
            if (_splashScreenButton != null) _splashScreenButton.onClick.RemoveListener(StartMenu);
            else Debug.LogWarning("[UIManager] No Button component found on the Splash Screen GameObject.");
        }
        #endregion

        #region Initialization
        private void Start() => _splashScreen.SetActive(true);
        public void StartMenu()
        {
            _splashScreen.SetActive(false);
            OpenMenu(MenuType.Main);
        }
        #endregion

        private void OpenMenu(MenuType menuType)
        {
            foreach (var menuData in _menuDataList)
            {
                if (menuData.menuType == menuType) 
                {
                    menuData.menuScreen.SetActive(true);
                    if (menuType == MenuType.Main) menuData.activateButton.gameObject.SetActive(false);
                } 
                else 
                {
                    menuData.menuScreen.SetActive(false);
                    if (menuData.menuType == MenuType.Main) menuData.activateButton.gameObject.SetActive(true);
                }
            }
        }
    }
}