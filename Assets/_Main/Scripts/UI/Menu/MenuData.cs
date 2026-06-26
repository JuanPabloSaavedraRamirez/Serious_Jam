using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    [System.Serializable]
    public class MenuData
    {
        public MenuType menuType;
        public GameObject menuScreen;
        public Button activateButton;
    }
}