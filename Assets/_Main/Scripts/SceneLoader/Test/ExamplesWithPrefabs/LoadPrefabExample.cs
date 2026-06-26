using System.Threading.Tasks;
using UnityEngine;
using SceneLoading;
using UnityEngine.UI;

public class LoadPrefabExample : MonoBehaviour
{
    [SerializeField] private GameObject ExamplePrefab;
    public Button button;

    public void OnEnable()
    {
        button.onClick.AddListener(OnPlayButtonPressed);
    }
    public void OnDisable()
    {
        button.onClick.RemoveListener(OnPlayButtonPressed);
    }

    public void OnPlayButtonPressed()
    {
        SceneLoader.RegisterMandatoryTask(LoadPrefabTask);
        SceneLoader.LoadScene("S_Gameplay");
    }

    private Task LoadPrefabTask()
    {
        GameObject instance = Instantiate(ExamplePrefab);

        if (instance.TryGetComponent<IAsyncInitializable>(out var initializable))
            return initializable.InitializeAsync();

        return Task.CompletedTask;
    }
}
