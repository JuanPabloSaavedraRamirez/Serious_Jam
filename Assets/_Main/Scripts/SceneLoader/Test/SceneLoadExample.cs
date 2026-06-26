using UnityEngine;
using UnityEngine.UI;
using SceneLoading;

public class SceneLoadExample : MonoBehaviour
{
    public Button button;
    public string sceneName;
    public void OnEnable()
    {
        button.onClick.AddListener(OnChargeScene);
    }

    public void OnDisable()
    {
        button.onClick.RemoveListener(OnChargeScene);
    }

    public void OnChargeScene()
    {
        SceneLoader.LoadScene(sceneName);
    }
}
